## Dependency Inversion Principle
A ve B sinifilari birbirini bilmesin. Araya interface koyarsak A ve B birbirini bilmez. A interfacei implement eden B sinifini kullanir. Bu sayede A ve B birbirini bilmez.
High level module low level moduleye bagimli olmamali. Ikisi de abstractiona bagimli olmalidir. Abstraction detaylara bagimli olmamali. Detaylar abstractiona bagimli olmalidir.
High level module controller, low level module service. Controller servicei kullanir. Service controlleri bilmez. Controller servicei interface uzerinden kullanir. Service interfacei implement eder. Bu sayede controller servicein detaylarini bilmez. Service controllerin detaylarini bilmez.
Soyurlama detaylara bagimli olmamali. Data nereden geliyor, bunlari bilmeyecek.
Hem test edilebilirligi yukseltiyor 
## Inversion of Control
A classi B classini kendi icinde newlemeyecek. B classi A classina disaridan gelebilir. Nasil gelebilir? Constructir injection, property injection, method injection. Constructor injection en cok kullanilan yontemdir. B classi A classina constructor ile gelebilir. B classi A classina property ile gelebilir. B classi A classina method ile gelebilir.
## Single Responsibility Principle
Bir component degismesi icin tek bir neden olmali.
## Persisted Ignorance
Entity Efcore ile ilgili bir kod olmayacak.
Entity hangi teknolojinin kullanildigini bilmeyecek.
Business logic data nereden geliyor bilmeyecek. Bu sayede projelerde ileriye donuk olarak teknolojiyi degistirmek kolaylasacak.
## Separation of Concerns
Kodun ilgi alanlarina göre birbirinden ayrilmasi. Controller, service, repository birbirinden ayrilacak. Controller sadece requestleri alacak ve servicei cagirecek. Service business logicleri yapacak ve repositoryi cagirecek. Repository data ile ilgili islemleri yapacak. Bu sayede kodun ilgi alanlari birbirinden ayrilmis olacak.
MVC design pattern buna örnektir.
## Clean Architecture
Business logic projeyi degerli hale getiren yerdir.
Projemizde en altta repository katmani var. Repositoryde yaptigmiz en ufak degisiklikte business logic etkileniyor.
Clean architureda repository service katmanini referans alacak. Bu durumda cache de, yada databasede ne oldugunu bilmiyor. CUnku referans almiyor.
Service katmani veri tabanina erismek icin interfacei tasiyacak.
Birde bunlarin ustunde uygulamanin service katmanindaki interfacei gordugunde bu interfacei hangi classin nesne örnegi
olusturacagini bilecek bir DI container frameworkine ulasak bir katmanimiz olacak oda api olacak. 
Api uygulamamiz hem product repository uzerinden databasee erisecek hemde service katmanina erissecek.
Infrastruce dedigimiz dbye giden kodlar, cachee giden kodlar,3rd party apiye giden kodlar en dista gerceklesiyor.
## CL-Domain
En temelde domain olacak. 
Entity,exception, yardimci en temel kodlar bulunur.
Herhangi bir dis dunyaya bagimliligi yoktur. Herhangi bir pakete bagliligini olmayandir. 
## CL-Usecase
Bir ust katmanda usecasei goruyoruz.
Kullanicinin yaptigi bir operasyondur.
Bir data almak, bir data guncellemek, bir data silmek, fatura olusturmak, sepete urun eklemek gibi islemler usecase olarak gorulebilir.
Kullanicinin yaptigi aksiyonlar hepsi birer usecase olarak gorulebilir.
Aslinda business logictir. Onion arc da adi applicationdur.
Domain ve bu usecase(application) katmanlari dis dunya hakkinda bilgi sahibi degildir.
Hangi dbden data geliyor, hangi kuyruga gididiyor, hangi cache kullaniliyor, hangi 3rd party apiye gidiyor bunlari bilmez. 
Ve infrastructuredan etkilenmezler.
## CL- En Dis Katmanlar
Persistance - DB ile ilgili kodlar
Infastructure - 3rd party api ile ilgili kodlar( Cache, email, sms, payment gateway)
Presentation - Kullaniciya gorunen katman. API, MVC, Blazor, WPF, WinForm, Console, Mobile App gibi.
Presentation hem infastructure hemde persistence erisebiliyor, referansini aliyor. 
Hem application service katmanina erisebiliyor, referansini aliyor. Indirekt yoldan domainide alabiliyor.
## Clean Architecture Project Structure
# Domain Layer
Entity,exception olacak.
Domain ve application layeri Core katmanini icerisinde birlestirecegiz
# Application Layer
Business codelar.
Istenirse zorunlu degil interfaceler olacak.(ProductRepository, IProductService gibi)
# Infrastructure Layer
Interface implementasyonlari olacak. (Efcore, Dapper, MongoDb, Redis, RabbitMq, Kafka gibi)
# User Interface Layer
Api, MVC, Middleware, Filters
Biz buna ayni zamanda 

## Iliskiler
En altta core olacak.
Ustte applicatoin olcak.
Persistence applicationi referans alacak.
Api ise DI Container olabilmesi icin interfacelerin bulunacagi applicationu ve repositorylerin bulunacagi persistence katmanini referans alacak.