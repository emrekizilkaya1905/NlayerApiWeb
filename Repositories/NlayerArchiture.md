# N-Layer Mimari Notlarım

Repository Design Pattern
## IGenericRepository
Burada iqueryable ile veritabanından veri çekme işlemlerini yapıyoruz. 
Bu sayede veritabanına sorgu atılmadan önce filtreleme, sıralama gibi işlemleri yapabiliyoruz.
Buradaki islemleri filtreleme islemlerini veri tabaninda yapilmasidir.
## Klasor yapısı
Functional bazda gidecegiz. Birbiriyle alakali klasorleri bir yerde toplayacagiz. Mesela Product ile alakali klasorler Product klasoru altinda olacak. Product klasoru altinda ProductController, ProductService, ProductRepository gibi klasorler olacak. Bu sayede kodlarimizi daha duzenli bir sekilde tutabilecegiz.