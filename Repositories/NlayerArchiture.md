# N-Layer Mimari Notlarım

Repository Design Pattern
## IGenericRepository
Burada iqueryable ile veritabanından veri çekme işlemlerini yapıyoruz. 
Bu sayede veritabanına sorgu atılmadan önce filtreleme, sıralama gibi işlemleri yapabiliyoruz.
Buradaki islemleri filtreleme islemlerini veri tabaninda yapilmasidir.
## Klasor yapısı
Functional bazda gidecegiz. Birbiriyle alakali klasorleri bir yerde toplayacagiz. Mesela Product ile alakali klasorler Product klasoru altinda olacak. Product klasoru altinda ProductController, ProductService, ProductRepository gibi klasorler olacak. Bu sayede kodlarimizi daha duzenli bir sekilde tutabilecegiz.
## ProductRepository
Custom metodlar icin yaptik bu repository'i
Miras yoluyla ProductRepository, GenericRepository'den türetiliyor. Bu sayede GenericRepository'deki tüm metodları kullanabiliyoruz. 
Ayrıca ProductRepository'de sadece Product ile alakalı metodları yazıyoruz. Mesela GetTopPriceProductsAsync metodu sadece Product ile alakalı bir metod. Bu sayede kodlarımız daha okunabilir ve anlaşılır oluyor.
Ama constructlar miras yoluyla gelmiyor. Bu yüzden Product repository contructorunda aldigim datayi GenericRepository constructoruna gönderiyoruz.
Generic service saglikli degildir. Servicelerin generic yapmak dogru degildir
