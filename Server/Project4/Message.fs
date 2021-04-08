namespace Twitter

module Message =

    //Client -> Server
    type LoginConfirmation = {confirm:string}//"confirm"
    type RegisterUser = {userId:string; live:bool}
    type LoginUser = {userId:string; live:bool}
    type Tweet = {tweetContent:string; userId:string}
    //userId subscribe accountId
    type AddSubscriber = {userId:string; accountId:string}
    type TweetsSubscribedTo = {userId:string}
    type GetMyTweets = {userId:string}
    type TweetsWithHashtag = {tag:string; userId:string}
    type TweetsWithMention = {userId:string}
    
    //Main -> Server
    type DisconnectUser = {userId:string}

    //Client -> Main
    //perfmetrics

    //Server -> Client
    type RegisterConfirmation = {confirm:string}// "confirm"
    type RepTweetsSubscribedTo = {list:List<string>}
    type RepGetMyTweets = {list:List<string>}
    type RepTweetsWithHashtag = {tag:string; list:List<string>}
    type RepTweetsWithMention = {list:List<string>}
    type Live = {tweetContent:string}