export interface Message {
  messageID: number,
  senderID: number,
  receiverID : number,
  senderKnownAs: string,
  senderUrl: string,
  receiverKnownAs: string,
  receiverUrl: string,
  messageContent: string,
  isRead: boolean,
  readDate: Date,
  sendDate : Date
}
