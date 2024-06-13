export class PersistData {

    public static put(key: string, value: string): void {
        localStorage.setItem(key, value);
    }

    public static get(key: string): string | null {
        return localStorage.getItem(key);
    }

    public static delete(key: string): void {
        localStorage.removeItem(key);
    }
}
