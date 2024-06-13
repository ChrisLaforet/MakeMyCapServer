export class FieldValidator {

    public static ValidatePassword(password: string | null | undefined): boolean {
        return !(!password || password.length <= 8);
    }
}
