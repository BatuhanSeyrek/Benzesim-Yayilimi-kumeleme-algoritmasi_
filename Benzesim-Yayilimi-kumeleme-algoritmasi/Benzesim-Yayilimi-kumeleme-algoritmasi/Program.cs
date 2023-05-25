using System;

class Program
{
    static void Main(string[] args)
    {
        // Veri noktalarinin cok boyutlu dizi olarak tanimlayip , icine degerlerini atadik
        double[,] veriNoktalari = {
            {1.2, 3.5},
            {2.0, 2.5},
            {1.8, 1.6},
            {4.0, 4.5},
            {3.5, 5.0}
        };


        double[,] benzesimMatrisi = new double[veriNoktalari.GetLength(0), veriNoktalari.GetLength(0)]; // Benzeşim matrisi 


        for (int i = 0; i < veriNoktalari.GetLength(0); i++)   // Benzesim matrisini hesaplamak icin ic ice dongu kullandik
        {
            for (int j = 0; j < veriNoktalari.GetLength(0); j++)
            {

                double mesafe = HesaplaOklidyenMesafesi(veriNoktalari[i, 0], veriNoktalari[i, 1], veriNoktalari[j, 0], veriNoktalari[j, 1]);// HesaplaOklidyenMesafesi metodunun degerlerini mesafe degiskenine atadik


                benzesimMatrisi[i, j] = mesafe;// Benzeşim matrisine mesafeyi kaydetmeyi sagalr
            }
        }


        int[] temsilciler = new int[veriNoktalari.GetLength(0)];  // temsilciler adli diziye veriNoktalari degerini atadik


        BenzesimYayilimiKumeleme(veriNoktalari.GetLength(0), benzesimMatrisi, temsilciler); //  BenzesimYayilimiKumeleme metotunu kullandik 

        for (int i = 0; i < temsilciler.Length; i++)// Kümeleri ekrana yazdır
        {
            Console.WriteLine("Veri noktası {0} kümeye ait temsilci: {1}", i, temsilciler[i]);//{0} degerini yerine i ifadesi gececek,{1} degerinin yerine temsilciler[i] ifadesi gelecek.
        }

        Console.ReadLine();
    }

    // Öklidyen mesafesini hesapla
    static double HesaplaOklidyenMesafesi(double x1, double y1, double x2, double y2)// Öklidyen mesafesini hesaplamayi saglayan metot
    {
        double dx = x2 - x1;
        double dy = y2 - y1;
        return Math.Sqrt(dx * dx + dy * dy);//Math.Sqrt ifadesi parantez içindeki ifadenin kokunun alinmasini saglar
    }
    static void BenzesimYayilimiKumeleme(int veriSayisi, double[,] benzesimMatrisi, int[] temsilciler) // BenzesimYayilimiKumeleme metodu
    {
        const double alfa = 0.5; // Öğrenme faktörü
        const int iterasyonSayisi = 100; // İterasyon sayısı

        // R matrisi (i, k): veri noktası i'nin k'ya benzesim mesajı
        double[,] R = new double[veriSayisi, veriSayisi];
        // A matrisi (i, k): veri noktası i'nin k'tan ağırlık mesajı
        double[,] A = new double[veriSayisi, veriSayisi];

        // İterasyonlar
        for (int iterasyon = 0; iterasyon < iterasyonSayisi; iterasyon++)
        {
            // Benzeşim mesajlarını güncelle
            for (int i = 0; i < veriSayisi; i++)
            {
                for (int k = 0; k < veriSayisi; k++)
                {
                    double maxMesaj = double.NegativeInfinity;
                    for (int j = 0; j < veriSayisi; j++)
                    {
                        if (j != k)
                        {
                            // R matrisinden en yüksek mesajı bul
                            double mesaj = A[j, k] + benzesimMatrisi[j, k];
                            if (mesaj > maxMesaj)
                                maxMesaj = mesaj;
                        }
                    }

                    // R matrisini güncelle
                    R[i, k] = (1 - alfa) * R[i, k] + alfa * (benzesimMatrisi[i, k] - maxMesaj);
                }
            }

            // Ağırlık mesajlarını güncelle
            for (int i = 0; i < veriSayisi; i++)
            {
                for (int k = 0; k < veriSayisi; k++)
                {
                    if (i != k)
                    {
                        double sum = 0.0;
                        for (int j = 0; j < veriSayisi; j++)
                        {
                            if (j != i && j != k)
                            {
                                // R matrisinden pozitif mesajları topla
                                double mesaj = Math.Max(0, R[j, k]);
                                sum += mesaj;
                            }
                        }

                        // A matrisini güncelle
                        A[i, k] = (1 - alfa) * A[i, k] + alfa * sum;
                    }
                }
            }
        }

        // Küme temsilcilerini belirle
        for (int i = 0; i < veriSayisi; i++)
        {
            double maxMesaj = double.NegativeInfinity;
            int temsilci = -1;
            for (int k = 0; k < veriSayisi; k++)
            {
                if (R[i, k] > maxMesaj)
                {
                    maxMesaj = R[i, k];
                    temsilci = k;
                }
            }
            temsilciler[i] = temsilci;
        }
    }
}

