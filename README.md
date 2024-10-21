# Fiber Games Game Developer Case

## Proje Açıklaması
Bu proje, 48 saatlik süre içinde Fiber Games için geliştirilen bir Hibrid-Casual Puzzle oyunudur. Oyun, oyuncuların belirli bir tile'a dokunarak coin stack'lerini gönderip, etrafındaki stack'ler arasında birleştirme işlemi gerçekleştirmesine olanak tanır. Proje, "Coins Shot 3D" referans oyunu göz önünde bulundurularak geliştirilmiştir.

## Oyun Özellikleri
- **Dikey Ekranda Oynama**: Oyun, dikey ekran formatında tasarlanmıştır.
- **Coin Stack Mekaniği**: Ekranın altında yenilenen coin stack'leri.
- **Merge İşlemi**: Aynı seviyedeki coin'leri birleştirme.
- **Komşu Tile Kontrolü**: Seçilen tile üzerindeki coin'in komşu tile'lardaki stack'ler ile etkileşimi.
- **Progress Bar**: Toplanan coin'lerin sayısı 10'a ulaştığında yok olur ve progress bar'ı doldurur.
- **Kapalı Tile'lar**: Bazı tile'lar kapatılabilir, bu tile'larla etkileşime geçilemez.

## Oyun Mekaniği
1. **Coin Stack'lerin Gönderilmesi**: Oyuncu, ekranın altındaki stack'i seçtiği tile'a göndermek için dokunur.
2. **Yol Kontrolü**: Seçilen tile ile spawn olan stack arasında bir yol varsa, stack hareket eder.
3. **Merge Kontrolü**: Hareket tamamlandıktan sonra, merge işlemi kontrol edilir.
4. **Kapalı Tile'lar**: Kapalı olan tile'lar ile etkileşimde bulunulamaz.

## Kullanılan Teknolojiler
- **Unity Engine**: Oyun motoru olarak kullanıldı (2021 versiyonu).
- **C#**: Oyun mantığı için programlama dili.

## Kullanılan Paketler
- **ErkanYaprak_UnityHelpers**: Oyun geliştirme sürecini kolaylaştırmak için çeşitli yardımcı scriptler içeren bir paket. Daha fazla bilgi için [buraya göz atabilirsiniz](https://github.com/nakrekarpay1245/ErkanYaprak_UnityHelpers.git).
- **Mask Transitions**: UI geçiş efektlerini yönetmek için kullanılan bir paket. Daha fazla bilgi için [buraya göz atabilirsiniz](https://assetstore.unity.com/packages/tools/gui/mask-transitions-295860).
- **DOTween (Hotween v2)**: Animasyon ve zamanlama için güçlü bir araçtır. Daha fazla bilgi için [buraya göz atabilirsiniz](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676).

## Kurulum
1. Bu projeyi klonlayın veya indirin:
   ```bash
   git clone https://github.com/nakrekarpay1245/FiberGamesCaseStudy
2. Unity Editor'ü açın ve projeyi yükleyin. 
3. Gerekli paketleri yükledikten sonra oyun sahnesini açarak çalıştırın.

## Oynanış

Oyuncular, coin stack'lerini grid üzerindeki tile'lara gönderir. Hedef tile'daki coin stack'i, komşu tile'lardaki stack'lerle birleştirerek puan kazanılır. Her merge işlemi, progress bar'ı doldurur.

## Geliştirici Hakkında

Ben Erkan Yaprak, oyun geliştirme alanında 4 yıllık bir deneyimim var. Bu süre zarfında, mobil ve PC platformları için çeşitli projelerde çalıştım. Oyun geliştirme sürecinde, modüler, yeniden kullanılabilir ve yüksek performanslı kod yazmaya odaklandım, özellikle Unity Engine ve C# ile. SOLID prensiplerine ve nesne yönelimli programlamaya bağlı kalarak, her projede en iyi kullanıcı deneyimini sunmayı hedefledim.

Unity Editor Scripting, sanatçılar ve tasarımcılar için işleri kolaylaştırma amacım doğrultusunda işimin ayrılmaz bir parçası haline geldi. Bugüne kadar GNARLY Game Studio'da "Agent Hunt" ve "Frontline Heroes" gibi oyunların geliştirilmesine katkıda bulundum ve OGEM Incubation Center'da geliştirdiğim bir projeyi Steam'de yayımlamaya hazırlanıyorum.

## Daha Fazla Bilgi

- [Kişisel Web Sitesi](https://erkanyaprak.w3spaces.com/)
- [GitHub](https://github.com/nakrekarpay1245)
- [Geliştirdiğim Projeler](https://erkanyaprak.itch.io/)
- [Steam'de Yayınlamayı Planladığım Proje](https://erkanyaprak.w3spaces.com/hard-deliver.html)

## İletişim

Herhangi bir sorunuz veya geri bildiriminiz varsa, bana [rknyprk79@gmail.com](mailto:rknyprk79@gmail.com) adresinden ulaşabilirsiniz.
