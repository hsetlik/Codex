using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Application.Extensions;
using Domain.DataObjects;
using Microsoft.AspNetCore.Identity;
using Persistence;

//initializing w/ dummy data
namespace Domain
{
    public static class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<CodexUser> userManager)
        {
            if (!userManager.Users.Any() && !context.Terms.Any())
            {
                //make some dummy users w/ whatever info
            var users = new List<CodexUser>
            {
                new CodexUser
                {
                    UserName = "jeff",
                    DisplayName = "Jeff",
                    Email = "jeff@test.com",
                    NativeLanguage = "en",
                    UserLanguageProfiles = new List<UserLanguageProfile>()
                },
                new CodexUser
                {
                    UserName = "hans",
                    DisplayName = "Hans",
                    Email = "hans@test.com",
                    NativeLanguage = "de",
                    UserLanguageProfiles = new List<UserLanguageProfile>()
                },
                new CodexUser
                {
                    UserName = "sonya",
                    DisplayName = "Sonya",
                    Email = "sonya@test.com",
                    NativeLanguage = "ru",
                    UserLanguageProfiles = new List<UserLanguageProfile>()
                 
                },
                new CodexUser
                {
                    UserName = "maria",
                    DisplayName = "Maria",
                    Email = "maria@test.com",
                    NativeLanguage = "es",
                    UserLanguageProfiles = new List<UserLanguageProfile>()
                }
            };
            //create each user on the server
            string[] langs = {"es", "en", "de", "ru"};
            for(int i = 0; i < 4; i++)
            {
                var profile = new UserLanguageProfile
                {
                    Language = langs[i],
                    UserId = users[i].UserName,
                    User = users[i]
                };
                users[i].UserLanguageProfiles.Add(profile);

                await userManager.CreateAsync(users[i], "Pa$$w0rd");
            }
            //some dummy words
            
            var contents = new List<ContentCreateDto>
            {
               new ContentCreateDto 
               { 
                    ContentName = "К...",
                    ContentType = "Article",
                    Language = "ru",
                    VideoUrl = "none",
                    AudioUrl = "none",
                    FullText = "Я помню чудное мгновенье= Передо мной явилась ты, Как мимолетное виденье, Как гений чистой красоты. В томленьях грусти безнадежной В тревогах шумной суеты, Звучал мне долго голос нежный И снились милые черты. Шли годы. Бурь порыв мятежный Рассеял прежние мечты, И я забыл твой голос нежный, Твой небесные черты. В глуши, во мраке заточенья Тянулись тихо дни мои Без божества, без вдохновенья, Без слез, без жизни, без любви. Душе настало пробужденье= И вот опять явилась ты, Как мимолетное виденье, Как гений чистой красоты. И сердце бьется в упоенье, И для него воскресли вновь И божество, и вдохновенье, И жизнь, и слезы, и любовь."
                },
                new ContentCreateDto
                {
                    ContentName = "Волк часть 1",
                    ContentType = "Article",
                    Language = "ru",
                    VideoUrl = "none",
                    AudioUrl = "none",
                    FullText = "Был один мальчик. И он очень любил есть цыплят и очень боялся волков. И один раз этот мальчик лег спать и заснул. И во сне он увидал, что идет один по лесу за грибами и вдруг из кустов выскочил волк и бросился на мальчика. Мальчик испугался и закричал= «Ай, ай! Он меня съест!» Волк говорит= «Постой, я тебя не съем, а я с тобой поговорю». И стал волк говорить человечьим голосом И говорит волк= «Ты боишься, что я тебя съем. А сам ты что же делаешь? Ты любишь цыплят?» — «Люблю». — «А зачем же ты их ешь? Ведь они, эти цыплята, такие же живые, как и ты. Каждое утро — пойди посмотри, как их ловят, как повар несет их на кухню, как перерезают им горло, как их матка кудахчет о том, что цыплят у нее берут. Видел ты это?» — говорит волк."
                },
                new ContentCreateDto
                {
                    ContentName= "Krokodil und Giraffe 1",
                    ContentType = "Article",
                    Language = "de",
                    VideoUrl = "none",
                    AudioUrl = "none",
                    FullText = "Mama Giraffe grübelt. 'Irgendwas ist doch heute', sagt sie 'Ja', sagt Papa Krokodil, 'das Gefühl habe ich auch. Aber was nur?' 'Hihi', kichern Krokira und Raffolo. Mama Giraffe sieht die beiden an. 'Ihr wisst doch was, oder?' 'Och, nö', sagt Krokira. 'Gar nix', sagt Raffolo und zieht an etwas Rotem, das unter der Tür hervorlugt. 'Was ist das?', fragt Mama Giraffe. 'Sieht aus wie eine Schnur!', meint Papa Krokodil. Mama Giraffe schaut aus dem Fenster. 'Wo führt sie wohl hin?' 'Sehen wir doch mal nach!', rufen Krokira und Raffolo."
                },
                new ContentCreateDto
                {
                    ContentName= "La Légende du Cid",
                    ContentType = "Article",
                    Language = "fr",
                    VideoUrl = "none",
                    AudioUrl = "none",
                    FullText = "La Légende du Cid (El Cid: La Leyenda) est un long métrage d'animation espagnol réalisé par José Pozo, sorti au cinéma en Espagne en 2003 et en France en 2004. Le film emploie la technique du dessin animé en deux dimensions. Il s'inspire de la vie de Rodrigo Díaz de Bivar, dit « le Cid », un héros de l'Espagne médiévale qui a inspiré de nombreuses œuvres dont la pièce Le Cid de Corneille. L'histoire commence à la cour du roi d'Espagne au xie siècle. Rodrigo Díaz de Bivar est un noble qui s'est lié d'amitié avec le fils du roi, Sanche, futur héritier du trône. Rodrigue s'éprend de Chimène, la fille du comte Gormas, mais celui-ci désapprouve cet amour. À la mort du roi Ferdinand, la stabilité politique du royaume est remise en cause, et plusieurs complots aboutissent à la mort de Sanche. Rodrigo est injustement accusé et exilé. Il entame une carrière militaire brillante, affronte les Maures et se couvre de gloire sous le nom de « Cid Campeador »."
                },
                new ContentCreateDto
                {
                    ContentName= "British Subjects- Wikipedia",
                    ContentType = "Article",
                    Language = "en",
                    VideoUrl = "none",
                    AudioUrl = "none",
                    FullText = "The term 'British subject' has several different meanings depending on the time period. Before 1949, it referred to almost all subjects of the British Empire (including the United Kingdom, Dominions, and colonies, but excluding protectorates and protected states). Between 1949 and 1983, the term was synonymous with Commonwealth citizen. Currently, it refers to people possessing a class of British nationality largely granted under limited circumstances to those connected with Ireland or British India born before 1949. Individuals with this nationality are British nationals and Commonwealth citizens, but not British citizens. \n The status under the current definition does not automatically grant the holder right of abode in the United Kingdom but most British subjects do have this entitlement. About 32,400 British subjects hold active British passports with this status and enjoy consular protection when travelling abroad; fewer than 800 do not have right of abode in the UK."
                },
                new ContentCreateDto
                {
                    ContentName= "España- Wikipedia",
                    ContentType= "Article",
                    Language= "es",
                    VideoUrl= "none",
                    AudioUrl= "none",
                    FullText = $"España se sitúa tanto al sur de Europa Occidental como en el norte de África. En Europa, ocupa la mayor parte de la península ibérica, conocida como España peninsular, y las islas Baleares (en el mar Mediterráneo occidental); en África se hallan las ciudades de Ceuta (en la península tingitana) y Melilla (en el cabo de Tres Forcas), las islas Canarias (en el océano Atlántico nororiental) y otras posesiones mediterráneas denominadas «plazas de soberanía». El municipio de Llivia, en los Pirineos, constituye un exclave rodeado totalmente por territorio francés. Completa el conjunto de territorios una serie de islas e islotes frente a las propias costas peninsulares. Tiene una extensión de 505 370 km²,11​ por lo que es el cuarto país más extenso del continente, tras Rusia, Ucrania y Francia y con una altitud media de 650 metros sobre el nivel del mar, uno de los países más montañosos de Europa. Su población supera los 47 millones de habitantes, aunque la densidad de población es reducida. El territorio peninsular comparte fronteras terrestres con Francia y con Andorra al norte, con Portugal al oeste y con el territorio británico de Gibraltar al sur. En sus territorios africanos, comparte fronteras terrestres y marítimas con Marruecos. Comparte con Francia la soberanía sobre la isla de los Faisanes en la desembocadura del río Bidasoa y cinco facerías pirenaicas De acuerdo con la Constitución, y según su artículo 3.1, «el castellano es la lengua española oficial del Estado. Todos los españoles tienen el deber de conocerla y el derecho a usarla».3​ En 2012, era la lengua materna del 82 % de los españoles.33​ Según el artículo 3.2, «las demás lenguas españolas serán también oficiales en las respectivas Comunidades Autónomas de acuerdo con sus Estatutos».3​ El idioma español o castellano, segunda lengua materna más hablada del mundo y con casi 600 millones de hispanohablantes,​ es uno de los más importantes legados del acervo cultural e histórico de España en el mundo. Perteneciente culturalmente a la Europa Latina y heredero de una vasta influencia grecorromana, España alberga también la cuarta colección más numerosa del mundo de sitios declarados Patrimonio de la Humanidad por la Unesco."
                }

            };
            foreach(var c in contents)
            {
                await context.CreateContent(c);
            }
            //add words to Db
            await context.SaveChangesAsync();
            //add contents to Db
            }
        }
    }
}