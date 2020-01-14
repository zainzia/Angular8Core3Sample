
import { Address } from "./Address";

import { Language } from "./language.module";

export interface UserProfile {

  UserProfileID: number,
  FirstName: string,
  LastName: string,
  PhoneNumber: string,
  Email: string,
  Username: string,
  Language: Language,
  Address: Address,
  UserId: string

}
