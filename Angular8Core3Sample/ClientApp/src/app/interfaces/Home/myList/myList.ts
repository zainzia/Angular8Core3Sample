
import { myListItem } from "./myListItem";

export interface myList {

    myListId: number;

    userAccountId: string;

    myListName: string;
    
    myListItems: myListItem[];
}
