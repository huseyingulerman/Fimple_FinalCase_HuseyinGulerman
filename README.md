
Bu projede küçük bir banka simülatörü kurmayı amaçladım. Kullanıcı kayıtlarını ıdentity kütüphanesini kullanarak yaptım. Generic repo kullandım.JWT kullandım. 3 farklı rol olarak yönetilebilmektedir. Projede register ve login olduktan sonra rollere göre controllerlara ulaşım gerçekleştirilebilmektedir.
API, Jwt ve Code-First yaklaşımıyla geliştirilen proje.
Language: C#
Database: Postgresql
ORM Tool: Entity Framework Core


![Adsız](https://github.com/huseyingulerman/Fimple_FinalCase_HuseyinGulerman/assets/125462131/71776285-5701-4d47-b250-56804f94952f)
![Adsız1](https://github.com/huseyingulerman/Fimple_FinalCase_HuseyinGulerman/assets/125462131/1f7cd859-4cfe-426a-9f1f-c3c3ceed0050)
![Adsız3](https://github.com/huseyingulerman/Fimple_FinalCase_HuseyinGulerman/assets/125462131/3e053fda-e2ff-42cc-8e67-622916fca4bc)
![Adsız4](https://github.com/huseyingulerman/Fimple_FinalCase_HuseyinGulerman/assets/125462131/9b099a76-029c-4d8c-91a4-c82ef4f26a5b)
![Adsız5](https://github.com/huseyingulerman/Fimple_FinalCase_HuseyinGulerman/assets/125462131/22b25906-e9fa-4564-8382-991636442503)


Giriş
Günümüzde, bankacılık hizmetleri giderek daha fazla dijitalleşmekte ve müşteri beklentileri sürekli olarak evrilmektedir. Bu değişim, bankacılık sistemlerinin güvenli, etkili ve kullanıcı dostu web servisleriyle donatılmasını zorunlu kılmaktadır. Bu ödev, öğrencilerin .NET platformu üzerinde RESTful API'ler geliştirme becerilerini geliştirmeyi amaçlamaktadır. 

Öğrenciler, gerçek dünya bankacılık işlevlerini simüle eden bir API oluşturarak, modern web teknolojileri ve uygulama güvenliği konularında derinlemesine bilgi ve deneyim kazanacaklardır.

Amaç
Bu ödevin temel amacı, öğrencilere .NET kullanarak RESTful API tasarlama ve geliştirme konusunda pratik deneyim kazandırmaktır. Öğrenciler, temel ve gelişmiş bankacılık işlevlerini içeren bir API geliştirirken, güvenlik, veri tabanı yönetimi, veri doğrulama ve hizmet odaklı mimari gibi konularda önemli beceriler edineceklerdir.

Ödev Kapsamı
Bu ödevde, öğrenciler aşağıdaki ana başlıklar altında bir REST API geliştireceklerdir:

Kullanıcı kaydı ve yönetimi.

Hesap işlemleri, bakiye sorgulama ve güncelleme.

Para transferleri ve işlem kayıtları.

Kredi başvurusu, ödeme planları ve sorgulama.

Otomatik ödeme ayarları ve yönetimi.

Müşteri destek talepleri ve takibi.

Öğrenciler, bu işlevler için gerekli olan REST API endpoint'lerini tasarlayacak ve .NET platformunda uygulayacaklardır. API'nin güvenliği, veri tabanı entegrasyonları ve kullanıcı deneyimi ödevin temel odak noktaları arasında yer alacaktır.

İşlevler ve Kurallar
Aşağıdaki işlevlerin her biri için belirlenen kurallar doğrultusunda bir REST API geliştirilmesi beklenmektedir:

1. Kullanıcı Yönetimi

İşlevler: Kullanıcı kaydı, girişi, bilgi güncellemeleri.

Kurallar:

Kullanıcıların şifreleri, güvenli bir şekilde hash'lenerek saklanmalı.

Kullanıcı girişlerinde, güvenli token tabanlı kimlik doğrulama (JWT) kullanılmalı.

Kullanıcıların sistemdeki rollerine göre farklı işlevlere erişimleri olmalıdır. Örneğin, "admin", "user", "auditor" gibi roller tanımlanabilir.

Yöneticilerin kullanıcı rollerini atayabilmesi, güncelleyebilmesi ve yönetebilmesi için API endpoint'leri eklenmelidir.

2. Hesap Yönetimi

İşlevler: Yeni hesap açma, bakiye görüntüleme ve güncelleme.

Kurallar:

Hesap açılışında minimum bakiye kontrolü yapılmalı. Bu miktar belirlenmeli ve yeni açılan hesaplar için zorunlu kılınmalıdır.

Hesap bakiyesi negatif olmamalı; bakiye güncellemeleri bu kurala göre yapılmalı.

Hesap açma ve bakiye güncelleme gibi işlemler belirli rollerle sınırlı olmalıdır.

3. Para Yatırma ve Çekme

İşlevler: İşlem kayıtları, bakiye kontrolü.

Kurallar:

Her işlem için miktar, işlem türü, tarih ve zaman gibi detaylar kaydedilmelidir.

Her işlem, veritabanı transaction'ları içinde gerçekleşmeli. Eğer bir işlem sırasında hata oluşursa, işlemin geri alınmasını sağlamak için transaction mekanizmaları kullanılmalıdır.

Para çekme işleminde, talep edilen miktar mevcut bakiyeden fazla ise işlem reddedilmelidir.

Birden fazla işlem aynı anda gerçekleştiğinde bakiye tutarlılığını korumak için uygun kilit mekanizmaları (lock mechanisms) kullanılmalıdır.

Para çekme ve yatırma işlemleri kullanıcının rolüne göre kısıtlanabilir. Örneğin, yüksek miktarlı işlemler sadece belli roller tarafından onaylanabilir.

4. Para Transferleri

İşlevler: İç ve dış transfer işlemleri.

Kurallar:

Transfer işlemleri, gönderen ve alıcı hesaplar için atomik işlemler olarak gerçekleşmeli. Eğer transferin bir kısmı başarısız olursa, tüm işlem iptal edilmelidir (atomicity).

Günlük ve işlem başına transfer limitleri kontrol edilmeli. Güvenlik ve kontrol amaçlı olarak kullanıcının günlük maksimum transfer miktarını sınırlandırmak.

5. Kredi İşlemleri

İşlevler: Kredi başvuruları, ödeme planları, durum sorgulamaları.

Kurallar:

Kredi Skoru Kontrolü: Başvuru sırasında müşterinin kredi geçmişi ve mali durumu değerlendirilmelidir.

Kredi Skoru Kontrol Servisi : Kredi skorunu hesaplar ve kredi başvurusunun onaylanıp onaylanmayacağına dair bir öneri sunar.

Kredi ödemeleri, belirlenen ödeme planına uygun şekilde takip edilmeli.

Kredi başvurularının onaylanması veya reddedilmesi belirli roller tarafından yapılmalıdır.

6. Otomatik Ödemeler

İşlevler: Düzenli fatura ödemeleri için otomatik ödeme ayarları.

Kurallar:

Otomatik ödemeler, yeterli bakiye kontrolüyle gerçekleşmeli.

Otomatik Ödemeler için Arka Plan Servisi ve Zamanlanmış Görevler

Düzenli aralıklarla, örneğin her gün belirli bir saatte, otomatik ödeme gereksinimlerini kontrol eder.

Yeterli bakiye varsa belirlenen ödemeleri gerçekleştirir.

Kullanıcılar, otomatik ödemeleri yönetebilmeli ve iptal edebilmeli.

7. Destek Talepleri

İşlevler: Müşteri destek talepleri ve durum takipleri.

Kurallar:

Destek talepleri, oluşturulma sırasına göre işlenmeli ve önceliklendirilmeli.

Destek taleplerinin önceliklendirilmesi ve işlenmesi farklı roller tarafından gerçekleştirilebilir.


Testler
Unit Testler
Para Yatırma ve Çekme: Her işlemin doğru kaydedildiğini ve bakiyenin uygun şekilde güncellendiğini kontrol edin. Transaction mekanizmaları ve kilit mekanizmalarının doğru çalıştığından emin olun.

Para Transferleri: Transfer işlemlerinin atomik olarak gerçekleştiğini ve limit kontrollerinin doğru uygulandığını doğrulayın.

Hesap Yönetimi: Yeni hesap açma, bakiye görüntüleme ve güncelleme işlevlerinin doğru çalışması için testler yapın. Minimum bakiye kontrolü ve negatif bakiye olmaması gibi iş kuralları test edilmelidir.

Integration Testler
1. Hesap Oluşturma

Adım: Kayıtlı kullanıcı için yeni bir banka hesabı oluşturun.

Kurallar:

Hesap oluşturma işlemi başarılı olmalı ve kullanıcıya hesap numarası verilmelidir.

Minimum bakiye kuralına uygun olarak hesap oluşturulmalıdır.

2. Para Yatırma

Adım: Oluşturulan hesaba belirli bir miktar para yatırın.

Kurallar:

Para yatırma işlemi başarılı olmalı ve hesap bakiyesi güncellenmelidir.

İşlem detaylarının doğru kaydedildiğini doğrulayın (miktar, tarih, işlem türü).

3. Para Transferi

Adım: Oluşturulan hesaptan başka bir hesaba para transferi yapın.

Kurallar:

Transfer işlemi başarılı olmalı ve her iki hesabın bakiyesi doğru şekilde güncellenmelidir.

Transfer limitleri ve kurallarına uygun olarak işlem yapılmalıdır.

İşlemin atomicity'sini (bütünlüğünü) doğrulayın; yani bir kısmı başarısız olursa, tüm işlemin iptal edildiğini kontrol edin.

Docker Entegrasyonu
Docker Entegrasyonu ve Konteynerizasyon

İşlevler: Uygulamanın Docker konteynerleri içinde çalıştırılması, Dockerfile oluşturulması.

Kurallar:

Her servis (API, veritabanı, vb.) için ayrı Docker konteynerleri oluşturulmalıdır.

Uygulamanın Docker üzerinde çalışabilmesi için gerekli Dockerfile'lar yazılmalıdır.

Docker Compose kullanılarak çoklu konteyner uygulamaları yönetilmelidir.

Kriterler
Değişken isimleri anlamlı, amaç neyse ona göre verilmiş.

Metot isimleri ile metodun amacını net ifade edilmiş.

Class'ların içindeki metot sayısı az ve amaca yönelik belirlenmiş.

İç içe if ler olmayacak. Complexity düşük tutulmuş.

Ayni kod parçasının tekrarlandığı duruma yer verilmemiş.

Kodu okumayı zorlaştıran conditional complexity yaratılmamış

Uzun metotlar (25 satırdan uzun olmamali.

Hardcoded değerler Const olarak isimlendirerek kullanılmış.

Aynı kodlama standardı tüm kodlarda uygulanmış. Farklı dosyalar arasında tutarsızlık yaratılmamış.

Class'ların içindeki metotlar tek bir sorumluluk alanında odaklı yazılmış.

Objectler arası bağımlılıklar enjekte edilmiş.

Dependency Injection kullanılmış.

Classlar arası bağımlılıkların en azda tutulmasına dikkat edilmiş.

İnterface gibi abstraction lar ihtiyaç olduğu için kullanılmış. Gereksiz abstraction eklenmemiş.

İnterface içeriği az ve tek sorumluluğa özgü metot imzası barındıracak şekilde tasarlanmış. Gereksiz design pattern kullanımı yapılmamış.

Gerçekten bir problem çözmek için kullanılmış.

Open-Closed Prensibine dikkat edilmiş. Kod genişlemeye açık, değişikliğe kapalı şeklinde tasarlanmış. Web/REST standartlarına dikkat edilmiş.
