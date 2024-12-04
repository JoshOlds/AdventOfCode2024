using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class day3 : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Day 3 Part 1: ");
        ExecutePart1();
        GD.Print("\nDay 3 Part 2: ");
        ExecutePart2();
    }

    // Part 1 of the Advent puzzle, print result to console
    public void ExecutePart1()
    {
        int sum = 0;
        
        List<Tuple<int, int>> multiplyPairs = GetMultiplyPairs();
        foreach (Tuple<int,int> pair in multiplyPairs)
        {
            sum += (pair.Item1 * pair.Item2);
        }
        
        GD.Print(sum);
    }

    // Part 2 of the Advent puzzle, print result to console
    public void ExecutePart2()
    {
        int sum = 0;
        
        List<Tuple<int, int>> multiplyPairs = GetOnlyEnabledMultiplyPairs();
        foreach (Tuple<int,int> pair in multiplyPairs)
        {
            sum += (pair.Item1 * pair.Item2);
        }
        
        GD.Print(sum);
    }

    /// <summary>
    /// Using RegEx, get all instances of a mul(xxx,yyy) match according to the Advent puzzle rules. 
    /// </summary>
    /// <returns>A List of tuples, where each tuple is the pair of values within the mul() function.</returns>
    private List<Tuple<int, int>> GetMultiplyPairs()
    {
        List<Tuple<int, int>> multiplyPairs = new List<Tuple<int, int>>();

        string input = GetPuzzleInput();
        // This RegEx pattern uses groups to isolate the first and second numbers within the matched text. This allows for easy access later.
        var regexPattern = @"mul\((\d{1,3}),(\d{1,3})\)";

        Regex rg = new Regex(regexPattern);
        MatchCollection regMatches = rg.Matches(input);
        
        // Each match in the MatchCollection is a mul(xx,yy) text that met the matching rules
        foreach (Match match in regMatches)
        {
            // group[1] is the first number in the match, group[2] is the second
            Tuple<int, int> pair = new Tuple<int, int>(match.Groups[1].Value.ToInt(), match.Groups[2].Value.ToInt());
            multiplyPairs.Add(pair);
        }

        return multiplyPairs;
    }

    /// <summary>
    /// Using RegEx, get all instances of a mul(xxx,yyy) OR do() OR don't() match according to the Advent puzzle rules. 
    /// </summary>
    /// <returns>A List of tuples, where each tuple is the pair of values within the mul() function, only when a prior do() was found.</returns>
    private List<Tuple<int, int>> GetOnlyEnabledMultiplyPairs()
    {
        List<Tuple<int, int>> multiplyPairs = new List<Tuple<int, int>>();

        string input = GetPuzzleInput();
        // This pattern will match on either a mul() function, or a do() or don't() function
        var regexPattern = @"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)";

        Regex rg = new Regex(regexPattern);
        MatchCollection regMatches = rg.Matches(input);

        // Start with multiply enabled according to the puzzle rules
        bool enabled = true;
        foreach (Match match in regMatches)
        {
            // If this match is a do() or don't(), enable/disable as appropriate
            if (match.Value.Contains("do()")) enabled = true;
            else if (match.Value.Contains("don't()")) enabled = false;
            // Otherwise this is a mul() match and we can process the numbers withing the mul() function if we are enabled
            else if (enabled)
            {
                Tuple<int, int> pair = new Tuple<int, int>(match.Groups[1].Value.ToInt(), match.Groups[2].Value.ToInt());
                multiplyPairs.Add(pair);
            }
        }

        return multiplyPairs;
    }

    private static string GetPuzzleInput()
    {
        return
            @"$]~?,'mul(268,621)why() mul(668,915)mul(887,633)from()!where(998,493)how(230,27) mul(940,760);'/when()*!;~mul(71,701)-{mul(448,270)mul(925,62)mul(414,959)when() % select()how() ;><mul(416,836)#where()>@*<>why()mul(703,154))&how()*]select()+>;(mul(480,365)mul(724,324)what()from()~!(:,who()mul(226,399)'~mul(212,790)#!&]-mul(876,690){mul(233,661)!   don't()~+-how()from(){mul(509,769)<when()}select()why()what()}mul(451,517)&*~select()>when()->mul(87,282) *$;how()what(),mul(617,13)+where() when()mul(657,513)#>(( %#&%]mul(320,507):$%from()select();!what()mul(724,705)how()$why()~select(),,how(959,725)mul(149,615)}mul(450,692)mul(170,701)how()$mul(59,770)how()!,[$[what()mul(436,740)#+%mul(14,222),'when()who()>+]+{mul(116,16):(~@#!{who()mul(349,134)%select()when():^?{from()@*mul(933,635)?mul(653,875)>:,(->how()'}mul(963,506)when(280,474)where())+when()select()mul(568,226)[;mul(181,212)%+where()mul(720,273)don't()%mul(954,188)how()^mul(518,842)}#%what():mul(990,844)what()?<when()]mul(151,203)( (+>why()when()~@mul(434,665)@]mul(603,96)#'}&}/mul(878,991)#-&select()select()!]what()*mul(10,199) #from()*%>];mul(197,660)-mul(921,878)where()what()-<from()when()mul(414,11)do()&}how() ++#mul(834,70)^++]do()*('(;%}+mul(229,122)<-mul(246,512)@#,~@)$mul(144,702),select()^^what()[where()where(989,666)~mul(641,578)$!mul(854,445)'{)when()mul(485,421),]mul(203,14)(mul(964,896)#what(219,318)}@why()when()}:#mul(40,891)$]mul(593,751)where()mul(755,69)what()'!mul(295,76)when()[~(}when(815,693)mul(115,764)what()from()mul(572,436)[#) #?where(641,661)+mul(582,92)[/&when()/</:: mul(653,493)mul(516,59)<'}[what(257,655)mul(15-^why(279,495)!]!#%?why()&mul(273,379)how()+){!?mul(317,463)when() ]!what()}mul(188,340)mul(274,697):@mul(546,487)!mul(412,582){^**{)?@mul(477,785)where()(?mul(376,158)@*don't()#^mul(178,395)/&[?%$/mul(411,217))[)(,'mul(538,300);&+~who()@mul(469,107)$:,?$~]when()>mul(447,619)who(300,310)}+-when()where()<mul(872,71)$why()'when()/,($when()mul(238,409))[):mul(60,472)*{do()@#how(520,828)from()>mul(727,831)from()^mul(982,400)/-who()+(>(&select()mul(508,679)^('*'mul(549,463)']*select()>'+?'mul(149,313),':)mul(226,577)^how()who()mul(347,528)]~<mul(633,771) ([;}}<}:{mul(271,100)where()$from(587,500)$*mul(974,539)}*?:[+/>mul(323,513)mul(204,877),,select(){mul(290,987)>when()select()~what()()mul(131,943)why(726,908)when()why()/&$what() >mul(629,63))&mul(281,287)!^ /mul(615,191)mul(932,78)%&)]when()/~%%#mul(769,253)+@}/what()@]mul(118,261)how()when();*why()?why()mul(886,136)from()why()>]+[$mul(825,656)>mul(150,646)+:# ))(!select(72,556)who(247,255)mul(560,142)<&&'&]}};mul(821,966(mul(965,238)^:!'^%)$(where()mul(59,811)how()/from()-<from()}[*when()mul(146,176)
/what()what(){?mul(835,756)how()]where()'<!'!#why(636,26)mul(655,281)-($<;#when()when()%'mul(553,980)+,why()+when()&how()%mul(976,110)<from()>mul(125,745)[{~@{/where()^when()mul(135,726)what()+>where(),}( mul(777,401);why()*what()>mul(273,895))who()what()^~}mul(262,790[<}?mul(63,369)how()>-@what()why()how()who()$don't()<~!why()@&{where()mul(826,512)-}&when():where()!&mul(450,950)mul(419,483)$from()how()do()who(178,159)mul(683,505)how()^mul(455,197)&select()/'what()'do() ^,[&^*mul(82,385)^;&from()!mul(204,650)what()<* mul(740,616)+(~*&+}>mul(960,112)*>}?^[(mul(514,803)why()}mul(444,392)#<&when(716,989)^mul(561,408<{don't()when()select();!)%%'*mul(443,654)how()!where(),-where()+{why()what()mul(662,268)]-!@)mul(112:@][[+how()&;how()mul(795,729)how())#$:~+where(869,115)^!do()(/+><#mul(825,562)#,when()@+?,<)how()mul(234,478)do(),$ [>mul(256,258)from()}{%how()#]mul(128,475) why()>:&mul(551,920;how()/select()&@'/$-?mul(162,185)-!)}select()*//$mul>$!where()/?/*}'mul(638,747)+who()(how()[from()(mul(703,27)select()when()when()mul(930,930)}don't()?[]why()^]who()when()[mul(776,659)</,-mul]mul(855,370)&-/}]]@'mul(591,102)from()mul(62,380){from()?)mul(44,125)?where()mul(149,129)from(),where()]- mulwhen()who(136,919){-+~how()when()>mul(210,715)$select()>mul%mul(731,236)!-{-?#&^~mul(287,758)'@?'how()from()mul(68,771)]]from()from();:-/mul(136,737)$what()why(771,834), }/:}mul(371,748)mul(476,154),!+why()from()select()%-?^mul(708,880)}why()from()}when()&> *mul(212,632)^^<#why()when()do()/;mul(864,803)mul(12,609:}what()what()what()],mul(731,140)^from()mul(826,563)(>~^'&?who()mul(315,307)how()+mul(562,147&%*%when())who()[^;}do()how(372,554)$:](from()!/mul[},+,mul(797,390)what(407,406)what()when(),:mul(819,147)[*mul~how(384,366))'mul(405,193)why()+who() who()**mul(862,595)select()mul(384,147)-{, what()what()do()@~+@/how()<when()how()-mul(952,896)?select()@%$mul(760,82)mul(574,265)don't()from()++what() &mul(594,364)# $~what()'where()mul(671,670)'#@!$#from()why()<mul(66,753)where(){select())mul(182,165)why()?when()>)%mul}mul(661,115)>?['do()mul(35'}@#>*~&mul(963,48) <mul(12,9):@when()when(){*how()?mul(195,687)!;who()~,<>>who(492,300)mul(524,257)#(~~mul(392,601)$ &;do()$%mul(482,146)mul(787,904)[~@!~-why()mul(229,907)where()who()(mul(357,675),mul(453,241)who())when()[~+mul(502,25)']]select() &[#@)mul(743?,@~mul(370,914)mul(824,56)+who(),how()'*from() @mul(330,830)why()what():/^/where()where()mul(400,891)&} mul(660,956)%;&select(693,80)};mul(974,635)+<]](mul(946,953)>who()&why()mul(753,83)what()],['$%mul(764,682,when()%+@';:why()%mul(656,275)mul(605,22)@[mul(688,929)what())~:mul(724,267)>--mul(480,89)mul(237,945)~@[(*mul(636,205)@{^]mul(441,199):?select()mul(563,508)mul(103,692)mul(52,210)@^who()*&!>$who()!mul(989,419),[<-select()who()select()&mul(613,351)!~;%^*:mul(580,115)mul(516,628)when()(^%,>:do()mul(286,899){-$?]+):when()mul(990,247):#select()#why(532,636)*%):'mul(86,546)how()mul(234,253)from()how(),,!mul(407,308))select()mul(240,422)mul(632,751)(mul(506,967)mul(548,606);,,&^mul(788,227);>:]%mul(111,533)mul(126,215)
]/mul(147,892)why():>^when()$>$&mul(303,773) how()don't()')where(184,885):)from()mulwho()*mul(876,387): why()mul(84,297)mul(315,341)what(690,97)?don't()]{!when()from(){what()mul(197,255)mul(855,166)(}~$mul+*>* }mul(64,27)^<~~*when()@mul(735,281)what()*$*'/}##$mul(959,785%from()![how();where(831,805)mul(411,892)(/from()#>do()who()/*]!what()?mul(629,216)}&,!mul(56,589){~+]&}(mul(922,707)what()# )# mul(880,584){]:mul@{where()*what()who(321,62)^%where()mul(557,582)<>^  mul(612,179!%&^:'?what()@mul(450,229)%mul(855,946),]who()%<@&?mul(487,412){]~;when()mul(583,461)select()select()what()<*%when()mul(181,955)do()$)/^::why()select()why()}mul(562,105)mul(751,114)')&,when(){>{mul(197,218)*mul(840,115)&}#<@$mul(712,194)+^};when()who()^}!don't()?#how()(what()mul(323,807)! -(-$/mul(591,888)!;mul(242,761)mul(619,608)-~who()&&*mul(518,709)what()! < ,where())%!mul(734<@/*who()$do()who()from()]$*how()+mul(599,103)mul(571,370)?&from()from()mul(409,542)@/,'({{mul(593,239)what()!(?mul(735,478)?mul(292,372)?-where()mul(642,507)why(){from()mul(889,136)how()/-*why()/from()/:-mul(413,457)[(where()(mul(202,599){};+when())what(547,682)#mul(719,361)]!**mul(976,244)'how(927,878)@when()'!mul(422,110)who();select()#why()where()@mul(224,849)(]& {where()>select()#mul(924,968),select()~when(272,716)]mulwhen()!;&do()*/?[mul(336,583)-{where()how(366,471)how()how(523,337):who()where(693,273)/mul(243,820)?^mul(853,171)++%mul(478,163)[-how()};]when()'?select()don't()#;where()&(;):what()mul(80)select()@when()mul(712,952)mul(262,548)from(165,167),@:mul(899,582)>$*$>^mul(686,196)>:*?(}mul(929,419)mul(998,303)$<,why();$mul(880,38)why(405,795){select()mul(215,866)'what()- %how():<don't()),+!-?who(971,350)[@mul(421,250)from()who() who()[;mul(131,149)$#from()when(412,746)mul(714,190);~{*}where()>who()[mul(639,467)<from()when()-%how()+*mul(65,827)mul(602,655)%what()/(mul(452,487)?do()what()?what()'-{)<[mul(493,789),who(339,679)/@?&mul(874,48 $+mul(26,167)*(mul(516,76)who())<where()]:?mul(869,874)where()>')why()do()mul(690,973)when()how(161,404)<from(){where();[mul(220,766)mul(897,316)how()#why()'$from()@mul(684,371))mul(159,439)^^~mul(446,162)<<{mul[{!what()!where()?mul(886,787):}: from(){]%/%mul;>&mul(202,173)}mul(428,938){+;from()>,'mul(999,421)select()/select())&![?from()select()mul(999%mul(225,521)what(),who()'*how()from()mul(348,864)% who()#how()>{' mul(388,836)^^what()@<:mul(963,504),when()%^}~^;~mul(778,115;?^where() )what()^{)*mul(632,400)who()[why()>what(603,334)who()^$mul(831,297)*-%mul(362,398%+~why(426,765)?^where()>mul(327,876)who()/?%:from()<select()mul(822,774)mul(577,987)&@^;-?mul(548,583)!)mul(531,201what(173,685)@who()>select())mul(765,201) *mul(445,679)(};who()who()}mul(753,171!{~[?who(): '%do()):when(197,925)mul(538,681)/,why()mul}{ (*[mul(268,860)mul(408,458)mul(266,226)[? how()!from(), &mul(135,62)from()select()mul(340,25)what()where()  what()'mul(545,879){how()}#![who()%mul(638,670)!;where()~select()mul(479,848)mul(109,148)'how()mul(416,15)from()@/+<!+@+mul(606,592)]mul(722,360)what()from(){&)@< mul(876,290)what()when()}]mul(983,542)mul(985,495)(??select(932,48)when()#when();mul(383,988?%what()mul(519,18)how()}}*when()when(977,898)when()~how(545,749)when()don't()mul(459,715){&>$mul(897,986)
@#*)^+?when()}#!/usr/bin/perl!mul(967,747)#~})what()$where()}(}mul(778,323)>:- }where()([&&mul(411,856)]-}mul(522,56)how()%+how()^;!-~do()~/)when()what()who()from()(mul(587,850)+what()who()from() ^who()-$mul(756,247) *what()mul(608,305)mul(684,567)when(366,8)]how(582,805)*#where()select()mul(766,413)?^ ^!%mul(496,516){who()'mul(314,640)%'&when()$$@from()*mul(278,921)#'{< $&%why()]mul(60,195)mul(817,525):@'#& (,mul(54,364)~~>!~+*?how()+mul(973,10<%why())>select()(mul(329,897)(how()}mul(106,169)@><)how()>who()'mul(326,837)[how()when()>mul(201,602)>don't()#^>)from()@]when();mul(913,201)!*>mul(346,648)where()when()*@<[do()?;]/!&!-mul(167,706)>{where()<{/select()/mul(690,524)''why()^>select();?mul(546,39);]when()/why()/!,from()where()mul(193,398)^/#when(),%)how()*mul(915,972)@{{*~mul(283,116)-mul(922,445)why()how()?:;>do()^^/~+{mul(218,56)}mul(4,84)[-;+>mul(625,977)#;how() *}when()from()}mul(238,60)!(~< >mul(518,730)@[(]mul(480,891)why()?,mul(739,807)^mul(248- >[$&mul(792,34)mul(429,330)why()from()^'what()mul(887,370) ^^do()&$$mul(878,990)select(988,557),:#[mul(935,519)(mul(627,252) )mul(370,34)mul(381,288)}(>?;}@>*$mul(205,870)}!from() /where()who():^!mul(925,721)<[select()+when()$+  {mul(58,683)who()where(574,239)what()when()'&[what(682,763),>mul(63,268)@*&<^mul(494*[/(:select()+$do()%from()])where()/ '}mul(605,58)who() from()^$who(907,117)mul(543,205)*'>{>) mul(203,851)]}mulselect()when()/when()?mul(108,926):[(>how():!)~^mul(435,239) how(917,967) why(740,79)mul(922,64$mul(718,98)mul(683,826)*)what()?when()what()@<):mul(685,595)/<{from()&select()mul(398,94)what()}#$%^]#mul(695,681)}{how()where(760,742)-]*mul(596,194)/when()who()[&,>$[?mul(576,392)-,&mul(726,538from()don't()mul(113,836)?what(158,900)?$who(),]}@(mul(589,112)'),)mul(513,45)how()'[,$when()!$ ~mul(552,559where()?]do()*select()[;;why()$mul(552,319)]~>where()where()~mul(203,829):mul(645,741)}> <from()/$+mul(855,250)&when()(-mul(542,287)how()<@what()$*where(598,870)mul(796,465)-)}mul(131,45){~)$]]+^where()mul(233,25)[?*:+how()-[+mul(380,251)@from()^~what()>-* 'mul(760,810)mul(484,762)-)&+mul(780,483), (+how()$-what()'mul(36,857),what(2,894)select(), !?mul(916,494)<{(&mul(965,701)]where()mul(393,273)#+from()mul(430,13) ++*how(626,996)#mul(399,24)when()#$:;{%,mul(940,82)^$!~what()>{/do()/select()mul(787,843)mul(521,553)%:/mul(208,377),#%@^+@^mul(987,863)!^select()who()-+do()where()&[<%&'where()~mul(366,478)!^why()?mul(57,407))~}mul(350,637)*how():*don't()>^?:!;]mulwhat()what()mul(740,90)<+select()where()! *-;*mul(117,865)-where()when()*>who()}what()mul(146,520)^]?*]+'mul(319,547):+from()(*%mul(555,298)-what()what()%what(521,465)mul(206,820)@*%: how()%[&mul(653,71)'from()mul(47,650):-who(203,412)who()&$!(<,mul(518,778)?why(721,752)why()-!*why()who()who()!mul(156,368)&from()[who()}who()-!mul(247where()'])mul(197,394)~<(]:do()select()where()!+(@when()who()/$mul(883,573)where()]+when()]>how()~'mul(625,896)from()from()~>*who()mul(354,189)what()what()<$;,&{~mul(49,83)who(699,589)from()[' &(,mul(482,242)@select()}mul(955,898)^],{mul(751,976)who(999,593)<do()~from()$mul$'@^$*,mul(286,266)&what()how()//{%mul(842,67)when()where(447,72)why()-^mul(341,551)
~!;$!)[select()mul(873,134)when()when() mul(904,816){$where()do()+^[)select(27,333)mul(38,988)!''why()*mul(672,64)do()select())select()!&-how()what()(mul(228,335))<what()from())where()&who()mul(968,542)(why()#*from()^<mul(657,700)@why()]mul(418,534),mul(742,417){how()-why()&select()mul(581,270) !what()how(),'~'mul(278,221)*^~$:<+how()'mul(844,971)how()//?@from()@-$:mul(216,756)++& (who()mul(4,859)when())-&{*}mul(519,145)$!why()~how()@/mul(640,127){how()!mul(713,178){mul(273,105)*mul(454,914)mul(94,834){+select()]?mul(980,950)select()&];how()< *what()do();(who()~}from()(/mul(796,944)how()why()[*<%mul(268,409)]^(mul(26,522)/^how(206,385)'why()?$why()'mul(94,238)~%?-'where())!mul(746,273)when(),/#*who()mul(170,879)(^^,/when()mul(981,865)mul(932,913):$)?mul(793,661)]*:,mul(60,636)(<[select()>?,(;mul(420,452)^})when()<(mul(636,434;<(how(94,282)who()+-&mul(783,657)[mul(847,658)~how(){}?mul(989,907)(!>/ $}>how()mul(558,445)}+when():from()when()mul(876,292)*<#@!'^*# mul(73,78)&&how()&&;]';mul(335,413)why())$;<mul(448,62))how()~#?select()who(148,699)#:<mul(503,275)where()~where()+select(),from(792,612)'why(585,938)}mul(209,138)who()$&@from()&<+mul{<mul(90,339)>why()don't()'!what()]mul(713,35)^:mul(786,539)<%what()how()%why()*mul(613,686)mul(875,836)^}when(){^< ,^don't()>where()select()why(),~what()' mul(225,88)([%from()~who()mul(64,166)mul(369,250)#<how()~from()-&{,when()mul(948,202)?select()how()when():mul(317,103)~{when())from()how()^mul(559,577))$~{:} -where() mul(853,535)why()?^mul(918,426)@@what(740,725)mul(159,486)select()+^)mul(422,850)*how()mul(532,887)&<'why()how()mul(870,395)what()how()from()}from()mul(420,808)where()  -how()'mul(574,731)how()(from()&where()'mul(809,68))where(),mul(554,141)mul(183,376)how()});%where()select()'>mul(960,433)&:]what()]when()$from()mul(979,105)%what()select()$(mul(213,297)mul(165,810)/&&%mul(724,982)+who(564,497)@mul(791,258)-from(155,445)why()]who()%&+)mul(108,490)who()when()])&:+:'*mul(650,770)mul(177,465)@?}from(),do():,from()how()what()when()where()mul^< %%select();what()}%,mul(752,362)!]~)when()[select(366,751);>mul(273,152)select()<<-/mul(511,598)?why()& how()$mul(448,432)>@mul(339,548)+when()~![^]&mul(647,93)&mul(267,37)@)select()mul(129,485<don't()mul(549,744)%,/;how()$do()from()^)where()-}mul(359,132)select() &@^#~%;]mul(60,683),$]mul(150,648)what(826,939)why()#{&$why()}}do()how()#mul(264,269)don't()-when()why()mul(978,583)+*;#mul(585,856)'&+'/[[mul(959,155)who()!mul(759,227):%!<!$mul(163,220){(*@ select()from()mul(886,772)why()('->*/^mul(565,645);when()how()/!],(mul(659,317)from()$why()[mul(576,674)from()mul(824,600)} where()%@]mul(928,200when(341,496)how()]#:/what()$why()?}mul(193,810)#@]^#,mul(874,478)why()where()''who():mul(285,401)why(868,116)when()mul(671,851)how(349,690)~mul(610,673)&]where()don't()$$?/mul(283,356)]where())mul(358,319)%from()!mul(423,507)^>who()who()mul(48,247)where();<where()*<$mul(836,551)+who()-~how()mul(166,387)[{,how()+mul(84,763) ( /^+mul(618,395)],);/mul(398,645)
mul(596,231)(}*}!don't()?^why()+,:who()&<<mul(899,148)~where()(,%:mul(264,322)select()%>'~select()#'&mul(8,476)*<;mul(750,268)what()[mul(876,751who(524,548)*mul(767,377)what(307,238)(,!]?mul(517,916)do()mul(801,959)where()mul(473,955)why()mul(904,980)how(),mul(236,191!&[(from()when(){mul(50,79);how()*mul$>*why()from()(mul(768,683)mul(899,105)/</where()how()mul(767,5)>mul(68,213)~+why()select()&mul(153,265)what()'mul(514,270)}what();why()~}'!,mul(531,368)mul(904,338&>:(]:#?select()mul(204,761)where()from()why()select()?what():$from()mul(233,16)~[@,-how()$-mul(581,325)when()~how();mul(110,774)**mul(108,350)?why()&*(&>mul(433,429)<$& }},}>mul(153what()%/:~/how()mul(353,227)-^>!*--select()when()#mul(619,64)(!?@%'>%select()mul(971,885))!@~mul(635,10)@[&?from()where()}mul(319,14){*/select()[mulwhere()when()why()/mul(891,25)#~who()&+from()mul(108,794)$^;when()[[mul(400,669)$:&^select()don't()what()@mul(135,346)select()!-><@,$-mul(221,166)*(, )mul(139,966)+>mul(92,236)[how()how()&{-why()%;:mul(349,310)+/^](mul(185,117)$:who()-/'mul(983,476)%where()%&when(65,34)mul(958,888)#'>select(102,250)&!how()+?/mul(604,95){* *+#?;@mul(84,761)!;#mul(662,291)'~select()!&'who():who()mul(385,648),^,what()'}),&~mul(239,520)&+;@:%%'who(966,358)what()mul(545,279)where()&mul(516,362)*~ :who()[,mul(287,643)^{mul(509,80)#?@{>%>/#don't()where()select()from()[$why()'mul(428,642)#what()mul(806,269)+@mul(793,59)-!where()?@mul(310,248)% ;where()(?!-how();mul(415,122)'^mul(517,755)mul(948,993)/%!$%{[/what()what()mul(234,164)}'from()how(){/-don't()when()when()%!who() $select()mul(117,925)$[&how()who(462,530)&how(17,552)<:!mul(154,694)#*-mul(906,910)*@;<*/+don't()+^--@}%when()}mul(547,650)why()[*#)where(),mul(844,920)mul(935,983)+mul(351,650)>how()who()!don't()-&mul(197,418)#)mul(662,690)how()&mul(342,84)what()~from()+%;+mul(782,56)!-what()[)^why()mul(858,919)$why()(when()mul(699,301)}where(){mul(357,611),@mul(814,378)+:mul(893,31)select()@{}/mul(203,692){why()!mul(163,169)-)&when(725,844)?{when()!'don't()when()!)]what():^select()mul(644,973)mul(953,102){where()where()@#<:+from()@mul(706,402)@?]mul(251,457)what(){-(~~>>select()?mul(681,425)from()@how()mul(807,455)how()who()from()]what())why(79,590)mul(624,732)mul(54,659)mul(59,977)@)@*},[do()mul(555,790)[/: &]mul(171,172);]?-?why()who()mul(888,721)don't()mul(607,283)who(166,719)-$;?$how()<who(),mul(352,251)mul(786,52)$/when())*mul(942,127)select()!{@[@&mul(189,105)!%where()--,how(){don't()~ ^+:[#+]mul(31,892)*+}do()(:#<^what()')+why()mul(525,350)when()why()~$;where()&how()?[do()mul(609,471)from()#*,*where()^}mul(669,739)/{mul(343,414):]when()+{~'mul(406,490)%#&~mul(894,362)[+#mul(901,351);,don't()mul(873,70);,how(167,232)when(351,648)#<]from(345,680)from(976,804)mul(48,225)~select()when()&{^(^mul(502,635)where()why()do());>}/!?/ mul(158,193)#-@~mul(276,84)mul(85,578)+}@?where()mul(923,250)?mul(579,411)*mul(932,39)/]#?[,where()?%]";
    }
}