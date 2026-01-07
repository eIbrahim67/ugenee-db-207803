import axios, { AxiosInstance } from 'axios';

export class ApiClient {
    private axiosInstance: AxiosInstance;
    private token: string | null = null;

    constructor(private baseUrl: string) {
        this.axiosInstance = axios.create({
            baseURL: this.baseUrl,
        });
    }

    async login(email: string, password: string): Promise<void> {
        try {
            const response = await this.axiosInstance.post('/api/Auth/login', { email, password });
            this.token = response.data; // The API returns the token directly as a string
            this.axiosInstance.defaults.headers.common['Authorization'] = `Bearer ${this.token}`;
            console.log('Successfully authenticated with API.');
        } catch (error: any) {
            if (error.response) {
                console.error('API Error Response:', JSON.stringify(error.response.data, null, 2));
            } else {
                console.error('API Error Message:', error.message);
            }
            throw error;
        }
    }

    private async request(method: string, url: string, data?: any): Promise<any> {
        try {
            const response = await this.axiosInstance.request({ method, url, data });
            return response.data;
        } catch (error: any) {
            if (error.response) {
                console.error(`API Error [${method} ${url}]:`, JSON.stringify(error.response.data, null, 2));
            } else {
                console.error(`API Error [${method} ${url}]:`, error.message);
            }
            throw error;
        }
    }

    async getCategories(): Promise<any[]> {
        return this.request('GET', '/api/Categories');
    }

    async createCategory(category: any): Promise<any> {
        return this.request('POST', '/api/Categories', category);
    }

    async getInstructors(): Promise<any[]> {
        return this.request('GET', '/api/Instructors');
    }

    async createInstructor(instructor: any): Promise<any> {
        return this.request('POST', '/api/Instructors', instructor);
    }

    async getCourses(): Promise<any[]> {
        return this.request('GET', '/api/Courses?pageSize=100');
    }

    async getCourse(id: number): Promise<any> {
        return this.request('GET', `/api/Courses/${id}`);
    }

    async createCourse(course: any): Promise<any> {
        return this.request('POST', '/api/Courses', course);
    }

    async createModule(module: any): Promise<any> {
        return this.request('POST', '/api/CourseContent/modules', module);
    }

    async createLesson(lesson: any): Promise<any> {
        return this.request('POST', '/api/CourseContent/lessons', lesson);
    }
}
