
import { UserProfile } from "./UserProfile";


export enum RegistrationResultEnum {
    UsernameAlreadyExists,
    EmailAlreadyRegistered,
    Succeeded,
    Failed
}


export interface RegistrationResult {
    Result: RegistrationResultEnum;
    UserProfile: UserProfile;
    Errors: string[];
}
