export class Mapper {
    static mapCourse(legacyCourse: any, categoryId: number, instructorId: number | null): any {
        return {
            icon: legacyCourse.icon?.displayName || legacyCourse.icon?.name || 'Code', // fallback to Code
            title: legacyCourse.title,
            titleAr: legacyCourse.titleAr,
            description: legacyCourse.description,
            descriptionAr: legacyCourse.descriptionAr,
            overview: legacyCourse.overview,
            overviewAr: legacyCourse.overviewAr,
            level: legacyCourse.level,
            levelAr: legacyCourse.levelAr,
            duration: legacyCourse.duration,
            durationAr: legacyCourse.durationAr,
            schedule: legacyCourse.schedule,
            scheduleAr: legacyCourse.scheduleAr,
            skills: legacyCourse.skills?.join('\n') || '',
            skillsAr: legacyCourse.skillsAr?.join('\n') || '',
            prerequisites: legacyCourse.prerequisites?.join('\n') || '',
            prerequisitesAr: legacyCourse.prerequisitesAr?.join('\n') || '',
            tools: legacyCourse.tools?.join('\n') || '',
            objectives: legacyCourse.objectives?.join('\n') || '',
            objectivesAr: legacyCourse.objectivesAr?.join('\n') || '',
            certification: legacyCourse.certification,
            certificationAr: legacyCourse.certificationAr,
            targetAudience: legacyCourse.targetAudience?.join('\n') || '',
            targetAudienceAr: legacyCourse.targetAudienceAr?.join('\n') || '',
            color: legacyCourse.color,
            active: legacyCourse.active,
            categoryId: categoryId,
            instructorId: instructorId,
        };
    }

    static mapCategory(legacyCategory: any): any {
        return {
            title: legacyCategory.title,
            titleAr: legacyCategory.titleAr,
            description: legacyCategory.description,
            descriptionAr: legacyCategory.descriptionAr,
        };
    }

    static mapInstructor(legacyInstructor: any): any {
        return {
            name: legacyInstructor.name,
            nameAr: legacyInstructor.nameAr,
            role: legacyInstructor.role,
            roleAr: legacyInstructor.roleAr,
            bio: legacyInstructor.bio,
            bioAr: legacyInstructor.bioAr,
            photo: legacyInstructor.photo,
            expertise: legacyInstructor.expertise?.join('\n') || '',
            expertiseAr: legacyInstructor.expertiseAr?.join('\n') || '',
            linkedIn: legacyInstructor.social?.linkedin || '',
            twitter: legacyInstructor.social?.twitter || '',
            website: legacyInstructor.social?.website || '',
        };
    }
    static mapModule(legacyModule: any, tocId: number): any {
        return {
            order: legacyModule.order,
            title: legacyModule.title,
            titleAr: legacyModule.titleAr,
            description: legacyModule.description,
            descriptionAr: legacyModule.descriptionAr,
            duration: legacyModule.duration,
            durationAr: legacyModule.durationAr,
            tableOfContentId: tocId,
        };
    }

    static mapLesson(legacyLesson: any, moduleId: number): any {
        return {
            order: legacyLesson.order,
            title: legacyLesson.title,
            type: legacyLesson.type,
            duration: legacyLesson.duration,
            durationAr: legacyLesson.durationAr,
            objectives: legacyLesson.objectives?.join('\n') || '',
            objectivesAr: legacyLesson.objectivesAr?.join('\n') || '',
            isPreview: legacyLesson.isPreview || false,
            moduleId: moduleId,
        };
    }
}
