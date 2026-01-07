import * as dotenv from 'dotenv';
import { ApiClient } from './services/api';
import { Mapper } from './services/mapper';
import * as path from 'path';

// Import legacy data
import * as legacyCourses from './legacy_data/courses';
import * as legacyInstructors from './legacy_data/instructors';
import * as legacyTocs from './legacy_data/tableOfContent';

dotenv.config();

async function runMigration() {
    const api = new ApiClient(process.env.API_BASE_URL || 'http://localhost:5134');

    try {
        console.log('--- Starting NuGenee Migration ---');

        // 1. Login
        await api.login(process.env.ADMIN_EMAIL!, process.env.ADMIN_PASSWORD!);

        // 2. Migrate Categories
        console.log('\nMigrating Categories...');
        const existingCategories = await api.getCategories();
        const categoryMap: Record<string, number> = {};

        for (const legacyCat of legacyCourses.COURSE_CATEGORIES) {
            let existing = existingCategories.find(c => c.title === legacyCat.title);
            if (!existing) {
                console.log(`Creating Category: ${legacyCat.title}`);
                existing = await api.createCategory(Mapper.mapCategory(legacyCat));
            } else {
                console.log(`Using Existing Category: ${legacyCat.title}`);
            }
            categoryMap[legacyCat.id] = existing.id;
        }

        // 3. Migrate Instructors
        console.log('\nMigrating Instructors...');
        const existingInstructors = await api.getInstructors();
        const instructorMap: Record<string, number> = {};

        for (const legacyInst of legacyInstructors.instructors) {
            let existing = existingInstructors.find(i => i.name === legacyInst.name);
            if (!existing) {
                console.log(`Creating Instructor: ${legacyInst.name}`);
                existing = await api.createInstructor(Mapper.mapInstructor(legacyInst));
            } else {
                console.log(`Using Existing Instructor: ${legacyInst.name}`);
            }
            instructorMap[legacyInst.id] = existing.id;
        }

        // 4. Migrate Courses
        console.log('\nMigrating Courses...');
        const existingCourses = await api.getCourses();
        const successCount = [];
        const failCount = [];

        for (const legacyCourse of legacyCourses.courses) {
            try {
                let courseId = 0;
                const existing = existingCourses.find(c => c.title === legacyCourse.title);

                if (existing) {
                    console.log(`Using Existing Course: ${legacyCourse.title}`);
                    courseId = existing.id;
                } else {
                    const catId = categoryMap[legacyCourse.categoryId];
                    const instId = instructorMap[legacyCourse.instructorId] || null;

                    if (!catId) {
                        console.warn(`Skipping course ${legacyCourse.title}: Category ID not found.`);
                        continue;
                    }

                    const mappedCourse = Mapper.mapCourse(legacyCourse, catId, instId);
                    console.log(`Migrating Course: ${legacyCourse.title}`);
                    const createdCourse = await api.createCourse(mappedCourse);
                    courseId = createdCourse.id;
                    successCount.push({ id: courseId, title: legacyCourse.title });
                }

                // 5. Migrate Table of Contents for this course
                const tocData = legacyTocs.tableOfContent.find(t => t.id === legacyCourse.id);
                if (tocData && courseId > 0) {
                    console.log(`  Checking TOC for: ${legacyCourse.title}`);
                    const tocId = courseId;

                    // Fetch existing course details to check for modules
                    const courseDetails = await api.getCourse(courseId);
                    const existingModules = courseDetails.tableOfContent?.modules || [];

                    for (const legacyModule of tocData.courseModules) {
                        try {
                            const existingModule = existingModules.find((m: any) => m.title === legacyModule.title);
                            let moduleId = existingModule ? existingModule.id : 0;

                            if (!existingModule) {
                                console.log(`    Creating Module: ${legacyModule.title}`);
                                const createdModule = await api.createModule(Mapper.mapModule(legacyModule, tocId));
                                moduleId = createdModule.id;
                            } else {
                                console.log(`    Using Existing Module: ${legacyModule.title}`);
                            }

                            // Migrate Lessons
                            if (moduleId > 0) {
                                const moduleDetails = existingModule || { lessons: [] };

                                for (const legacyLesson of legacyModule.lessons) {
                                    const existingLesson = moduleDetails.lessons?.find((l: any) => l.title === legacyLesson.title);
                                    if (!existingLesson) {
                                        try {
                                            await api.createLesson(Mapper.mapLesson(legacyLesson, moduleId));
                                            console.log(`      Created Lesson: ${legacyLesson.title}`);
                                        } catch (lessonError: any) {
                                            console.error(`      Failed to create lesson ${legacyLesson.title}:`, lessonError.message);
                                        }
                                    }
                                }
                            }
                        } catch (moduleError: any) {
                            console.error(`    Failed to process module ${legacyModule.title}:`, moduleError.message);
                        }
                    }
                }

            } catch (err: any) {
                console.error(`Failed to migrate course ${legacyCourse.title}:`, err.response?.data || err.message);
                failCount.push({ title: legacyCourse.title, error: err.message });
            }
        }

        // Final Report
        console.log('\n--- Migration Report ---');
        console.log(`Successful: ${successCount.length}`);
        console.log(`Failed: ${failCount.length}`);

        if (successCount.length > 0) {
            console.log('\nCreated Records:');
            successCount.forEach(s => console.log(`[${s.id}] ${s.title}`));
        }

        if (failCount.length > 0) {
            console.log('\nFailures:');
            failCount.forEach(f => console.log(`${f.title}: ${f.error}`));
        }

    } catch (error) {
        console.error('Migration crashed:', error);
    }
}

runMigration();
