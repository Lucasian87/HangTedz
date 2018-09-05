using UnityEngine;
public class WordsDataBase
{

    public static string[] EasyWords = new string[] { "red", "hello", "you", "narnia", "awesome", "good", "green","yellow","minion","movie","fuel","pink","dot","cat",
    "mobile","purple","white","apple","food","games","doctor","what","miami","mom","sun","letter","error","save","easy","manager","cyclop","urban","main","code","help",
    "tools","info","view","file","test","asia","random","last","camper","start","star","moon","team","group","people","obama","putin","chine","color","visual","ball",
    "list","music","sky","space","crazy","culture","word","world","work","blue","small","high","low","bottom","left","right","front","upper","eye","banner","steam",
    "object","base","shape","call","run","walk","idle","see","open","close","add","exit","atlas","super","mega","twitter","gross","earn","dollar","money","america","crazy",
    "sister","yolo","fun","radio","boss","men","women","son","cousin","web","deep","true","all","you","need","more","Enjoy","guide","past","ago","video","image","sound","end",
    };

    public static string[] MediunWords = new string[] { "godzilla","naughty", "Real Madrid","Barcelona","quick","Anita", "hangman","X Ray","Midle","Microchip","computer","wolverine", "network","difficult","hardwell","hardware","keyboard","hoverboard","wearable","friends","swimming","service","manager","score","different","facebook","instagram",
    "generally","manipulation","vunerable","column","unity","studio","camera","pause","quality","processes","iphone","android","window","opera","catching","package","creator",
    "script","database","component","solution","explorer","visual","settings","setup","recent","regular","price","asset","build","target","suitable","devices","batching","media",
    "country","animals","good","question","league","brother","vector","warning","Clarify","phone","desktop","update","inclusion","history","present","million","years","material",
    "coca cola","pepsi","kid","cero","intro",};

    public static string[] HardWords = new string[] { "embezzle","flabbergasted", "gratuitous","haughty","hypocrisy","obsequitous", "penchant","refurbish","serendipity","superfluous","sycophant","zenith","tinkiwinki","international","cacophony","difficult","conflagration","deleterious","desiccated","dissemble","ebullient","wearable","execrable","expunge","guaranteed","creativity","Development","Innovative","battlefield","quickly","triangles","significantly","platforms",
 "fallacious","hackneyed","gynecologist","elephant","fragmented","microsoft","Rectangular","Cylindrical","compatibility","extension","resolution","optimization","snapchat","champion",
   "brothers","orthodontics","manicurist","invertebrate","analgesic","protuberance","Supported","publishers","seamlessly","phrases","Oxford","Dictionaries","individual","through", "authority","unsurpassed","pronunciation","informative","entertaining","commentaries","incorporated", };

    public static string[] FutconkWords = new string[] { "conker","berri","panther king","ze professor","professor von kriplespac","don weaso","wayne and the wankas","haybot","the big big guy","the great mighty poo","buga the knut",
    "count batula","little girl","heinrich","the weasal throne guards","birdy","gargoyle","mrs queen bee","gregg the grim reaper","marvin","franky","ron and reg","mr king bee","sunflower","bugger lugs and the cows","catfish","dogfish","carl","quentin",
   "fire imps","the ugas","bloopie the baby dinosaur","fangy","jugga","the corporal","the tediz","rodent","anti gravity chocolate","squirrel tails","rock people","cash","zombie","mr barrel","honker the skunk",
    "tickly bees","reagan","bullfish","earthworm","villager","evil acorn","lady cogs","dung beetle","wise guys","weasel mafia","marta","kulas of conk","conkerzbackfurday","uga buga","paulie","barry",
    "jack","clang","the survivors","cheese","tnt imp","wasp","tedi general",
    };

    public static string GetWord(DifficultMode mode)
    {
        string[] ar = null;
        switch (mode)
        {
            case DifficultMode.EASY:
                ar = EasyWords;
                break;
            case DifficultMode.MEDIUM:
                ar = MediunWords;
                break;
            case DifficultMode.HARD:
                ar = HardWords;
                break;
            case DifficultMode.FUTBOL:
                ar = FutconkWords;
                break;
            default:
                ar = MediunWords;
                break;
        }
        int max = Random.Range(1, ar.Length);
        int min = Random.Range(0, (max - 1));
        int r = Random.Range(min, max);
        return ar[r];
    }
}