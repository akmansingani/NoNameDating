import { Photo } from './photo';

export interface User {
  userID: number;
  userName: number;
  knownAs: number;
  age: number;
  gender: string;
  createdDate: Date;
  activeDate: Date;
  introduction: string;
  lookingFor: string;
  city: string;
  country: string;
  photourl: string;
  photos: Photo[];
}
