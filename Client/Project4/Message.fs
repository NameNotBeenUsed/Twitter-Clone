namespace Twitter

module Message =

    //Client -> Server
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
    type TweetsTimeDiff = {tweetsTimeDiff:float} 
    type QueriesSubscribedtoTimeDiff = {queriesSubscribedtoTimeDiff:float} 
    type QueriesHashtagTimeDiff = {queriesHashtagTimeDiff:float} 
    type QueriesMentionTimeDiff = {queriesMentionTimeDiff:float}
    type QueriesMyTweetsTimeDiff = {queriesMyTweetsTimeDiff:float}

    //Server -> Client
    type RegisterConfirmation = {confirm:string}// "confirm"
    type LoginConfirmation = {confirm:string}//"confirm"
    type RepTweetsSubscribedTo = {list:List<string>}
    type RepGetMyTweets = {list:List<string>}
    type RepTweetsWithHashtag = {tag:string; list:List<string>}
    type RepTweetsWithMention = {list:List<string>}
    type Live = {tweetContent:string}