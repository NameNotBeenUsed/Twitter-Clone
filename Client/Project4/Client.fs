namespace Twitter

open System
open Akka.Actor
open Akka.FSharp

module Client =
    
    type Client(userId:string, noOfTweets:int, noToSubscribe:int, existingUser:bool, clientSystem:ActorSystem) = 
        inherit Actor()
        
        let server = clientSystem.ActorSelection("akka.tcp://serverSystem@localhost:8778/user/server")
        let perfmetrics = clientSystem.ActorSelection("/user/perfmetrics")

        let stopWatch_tweetsTimeDiff = System.Diagnostics.Stopwatch.StartNew()
        let stopWatch_queriesSubscribedtoTimeDiff = System.Diagnostics.Stopwatch.StartNew()
        let stopWatch_queriesHashtagTimeDiff = System.Diagnostics.Stopwatch.StartNew()
        let stopWatch_queriesMentionTimeDiff = System.Diagnostics.Stopwatch.StartNew()
        let stopWatch_queriesMyTweetsTimeDiff = System.Diagnostics.Stopwatch.StartNew()

        //let whereis(name:string) = 
            //system.ActorSelection("user/" + name)

        let randomizer(length:int) : string = 
            let mutable tweetcontent: string = ""
            let randomCh = 
                let baselist = "abcdefghijklmnopqrstuvwxyz"
                let random = Random()
                fun () -> baselist.Chars (random.Next(baselist.Length-1))
            let array = Array.zeroCreate length
            for i = 0 to length - 1 do
                array.[i] <- randomCh()
            let tweetcontentStr = new string(array)
            tweetcontentStr

        let loginHandler() =
            //let server = whereis("server")
            let logmsg = {Message.LoginUser.userId = userId; Message.LoginUser.live = true}
            server <! logmsg
            for i in 1..5 do
                let content = randomizer(8)
                let tweet = {Message.Tweet.tweetContent = (sprintf "user %s tweeting that %s does not make sense" userId content); Message.Tweet.userId = userId}
                server <! tweet

        let rec generateSubList(count:int, noOfSubs:int, list:List<int>) = 
            if count = noOfSubs then
                count::list
            else
                generateSubList(count + 1, noOfSubs, count::list)

        let handleZipfSubscribe(subList:List<int>) = 
            for id in subList do
                let addSubMsg = {Message.AddSubscriber.userId = userId; Message.AddSubscriber.accountId = string(id)}
                server <! addSubMsg

        let handleRetweet(list:List<string>) = 
            //Retweet tweet subscribed
            if  not(list.IsEmpty) then
                let rtMsg = {Message.Tweet.tweetContent = (sprintf "%s -RT" list.Head); Message.Tweet.userId = userId}
                server <! rtMsg
                //printfn "User %s :- Retweet %s -RT" userId list.[0]
                let getMyTweetMsg = {Message.GetMyTweets.userId = userId}
                server <! getMyTweetMsg
            stopWatch_tweetsTimeDiff.Stop()
            let tweetsTimeDiffMsg = {Message.TweetsTimeDiff.tweetsTimeDiff = stopWatch_tweetsTimeDiff.Elapsed.TotalMilliseconds}
            perfmetrics <! tweetsTimeDiffMsg

        let handleQueriesSubscribedto(list:List<string>) = 
            if not(list.IsEmpty) then
                printfn "User %s :- Tweets Subscribed To" userId
                for tweet in list do
                    printfn "%s" tweet
            stopWatch_queriesSubscribedtoTimeDiff.Stop()
            let queriesSubscribedtoTimeDiffMsg = {Message.QueriesSubscribedtoTimeDiff.queriesSubscribedtoTimeDiff = stopWatch_queriesSubscribedtoTimeDiff.Elapsed.TotalMilliseconds}
            perfmetrics <! queriesSubscribedtoTimeDiffMsg

        let handleQueriesHashtag(tag:string, list:List<string>) =
            if not(list.IsEmpty) then
                printfn "User %s :- Tweets With %s" userId tag
                for tweet in list do
                    printfn "%s" tweet
            stopWatch_queriesHashtagTimeDiff.Stop()
            let queriesHashtagTimeDiffMsg = {Message.QueriesHashtagTimeDiff.queriesHashtagTimeDiff = stopWatch_queriesHashtagTimeDiff.Elapsed.TotalMilliseconds}
            perfmetrics <! queriesHashtagTimeDiffMsg

        let handleQueriesMention(list:List<string>) = 
            if not(list.IsEmpty) then
                printfn "User %s :- Tweets With @%s" userId userId
                for tweet in list do
                    printfn "%s" tweet
            stopWatch_queriesMentionTimeDiff.Stop()
            let queriesMentionTimeDiffMsg = {Message.QueriesMentionTimeDiff.queriesMentionTimeDiff = stopWatch_queriesMentionTimeDiff.Elapsed.TotalMilliseconds}
            perfmetrics <! queriesMentionTimeDiffMsg

        let handleGetAllMyTweets(list:List<string>) = 
            if not(list.IsEmpty) then
                printfn "User %s :- All my tweets" userId
                for tweet in list do
                    printfn "%s" tweet
            stopWatch_queriesMyTweetsTimeDiff.Stop()
            let queriesMyTweetsTimeDiffMsg = {Message.QueriesMyTweetsTimeDiff.queriesMyTweetsTimeDiff = stopWatch_queriesMyTweetsTimeDiff.Elapsed.TotalMilliseconds}
            perfmetrics <! queriesMyTweetsTimeDiffMsg

        let clientHandler() =
            //Subscribe
            if noToSubscribe > 0 then
                let subList = generateSubList(1, noToSubscribe, [])
                handleZipfSubscribe(subList)
                
            //let stopWatch_tweetsTimeDiff = System.Diagnostics.Stopwatch.StartNew()
            stopWatch_tweetsTimeDiff.Restart()
            let rand = new Random()
            //Mention
            let userToMention = rand.Next(int(userId)) + 1
            let menMsg = {Message.Tweet.tweetContent = (sprintf "user %s tweeting @%d" userId userToMention); Message.Tweet.userId = userId}
            server <! menMsg

            //Hashtag
            let hashtagMsg = {Message.Tweet.tweetContent = (sprintf "user%s tweeting that #COP5615isgreat" userId); Message.Tweet.userId = userId}
            server <! hashtagMsg

            //Send tweets
            for i in 1..noOfTweets do
                let ranMsg = randomizer(8)
                //printfn "user%s tweets that %s doesn't make sense." userId ranMsg
                let tweetMsg = {Message.Tweet.tweetContent = (sprintf "user%s tweets that %s doesn't make sense." userId ranMsg); Message.Tweet.userId = userId}
                server <! tweetMsg
                
            //Retweet
            //query tweets userId subscribes to
            let subQueryMsg = {Message.TweetsSubscribedTo.userId = userId}
            server <! subQueryMsg

            //Queries
            //Subscribed to
            //done during Retweet
            stopWatch_queriesSubscribedtoTimeDiff.Restart()
                
            //Hashtag
            stopWatch_queriesHashtagTimeDiff.Restart()
            let hashtagQueryMsg = {Message.TweetsWithHashtag.tag = "COP5615isgreat"; Message.TweetsWithHashtag.userId = userId}
            server <! hashtagQueryMsg
                
            //Mention
            stopWatch_queriesMentionTimeDiff.Restart()
            let mentionQueryMsg = {Message.TweetsWithMention.userId = userId}
            server <! mentionQueryMsg

            //Get my tweets
            stopWatch_queriesMyTweetsTimeDiff.Restart()
            let getMyTweetMsg = {Message.GetMyTweets.userId = userId}
            server <! getMyTweetMsg
                

        //do
        let register() = 
            let regmsg = {Message.RegisterUser.userId = userId; Message.RegisterUser.live = true}
            server <! regmsg
            clientHandler()

        //do
        do register()

        override x.OnReceive message = 
            match box message with
            | :? Message.Live as msg ->
                printfn "%s" (sprintf "User %s :- Live View ----- %s" userId msg.tweetContent)
            
            | :? Message.RegisterConfirmation as msg ->
                printfn "User %s :- registered on server" userId

            | :? Message.RepTweetsSubscribedTo as msg ->
                handleRetweet(msg.list)
                handleQueriesSubscribedto(msg.list)

            | :? Message.RepGetMyTweets as msg ->
                handleGetAllMyTweets(msg.list)

            | :? Message.RepTweetsWithHashtag as msg ->
                handleQueriesHashtag(msg.tag, msg.list)
            
            | :? Message.RepTweetsWithMention as msg ->
                handleQueriesMention(msg.list)
            
            | :? Message.LoginConfirmation as msg ->
                printfn "User %s: reconnected" userId
                loginHandler()

            | _ -> x.Unhandled message


