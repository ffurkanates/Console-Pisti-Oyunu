using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsolPisti
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OyunPisti oyunPisti = new();
            while (true)
            {
                Console.Write("\n1-Oyuna Başla    :\n2-Sonuç   :\n3-Oyundan Çıkış  :\nİşlem seçiniz ===> ");
                string secim = Console.ReadLine();
                if (secim == "1")
                {
                    oyunPisti.Temizleme();
                    oyunPisti.OyuncuBilgi();
                    oyunPisti.DesteOlustur();
                    oyunPisti.KartKarma();
                    oyunPisti.OrtayaKartKoy();
                    oyunPisti.SonKartAdi();
                    while (true)
                    {
                        if (oyunPisti.toplamdeste.Count != 0 && oyunPisti.oyuncu1eli.Count == 0 && oyunPisti.oyuncu2eli.Count == 0)
                        {
                            Console.WriteLine("Oyuncularda kart yok.... oyunculara kartlar dağıtılıyor....");
                            oyunPisti.KartDagıt();
                            oyunPisti.SonKartAdi();
                        }
                        oyunPisti.KartAt(oyunPisti.oyuncu1eli, oyunPisti.oyuncu1adi);
                        oyunPisti.KartTopla(oyunPisti.toplanan1, 1, oyunPisti.pistisayi1);
                        if (oyunPisti.toplamdeste.Count == 0 && oyunPisti.oyuncu1eli.Count == 0 && oyunPisti.oyuncu2eli.Count == 0 && oyunPisti.masa.Count != 0)
                        {                                                               //oyun bitişi kontrolü
                            oyunPisti.MasadaKalanKartlar();
                            break;
                        }
                        oyunPisti.SonKartAdi();

                        oyunPisti.KartAt(oyunPisti.oyuncu2eli, oyunPisti.oyuncu2adi);
                        oyunPisti.KartTopla(oyunPisti.toplanan2, 2, oyunPisti.pistisayi2);
                        if (oyunPisti.toplamdeste.Count == 0 && oyunPisti.oyuncu1eli.Count == 0 && oyunPisti.oyuncu2eli.Count == 0 && oyunPisti.masa.Count != 0)
                        {                                                           //oyun bitişi kontrolü
                            oyunPisti.MasadaKalanKartlar();
                            break;
                        }
                        oyunPisti.SonKartAdi();
                    }
                    oyunPisti.Sonuc();
                }

                if (secim == "2")
                {
                    oyunPisti.KartGecmisi();
                }
                if (secim == "3")
                {
                    break;
                }
            }
        }
    }
    class OyunPisti
    {
        public List<string> toplamdeste = new List<string>();          //"♥ kupa", "♠ maça", "♦ karo", "♣ sinek"
        public List<string> oyuncu1eli = new();
        public List<string> oyuncu2eli = new();
        public List<string> toplanan1 = new();
        public List<string> toplanan2 = new();
        public List<string> masa = new();
        public List<string> cekilenkartlar = new();
        public List<int> sonalankontrol = new();
        public List<int> pistisayi1 = new();
        public List<int> pistisayi2 = new();
        public int puan1 = 0;
        public int puan2 = 0;
        public int gun = 0;
        public string oyuncu1adi = null;
        public string oyuncu2adi = null;

        public void OyuncuBilgi()
        {
            Console.Write("Adınızı Ve Soyadınızı giriniz : ");
            string isim = Console.ReadLine();
            this.oyuncu1adi = isim.Split(" ")[0];
            this.oyuncu2adi = isim.Split(" ")[1];
            Console.Write("Doğum tarihinizi giriniz (01.01.1010) ==>  ");
            string tarih = Console.ReadLine();
            this.gun = Convert.ToInt32(tarih[..2]);
        }
        public void DesteOlustur()
        {
            List<string> kartsembol = new List<string>() { "♥", "♠", "♦", "♣" };
            List<string> kartsayi = new List<string>() { "2", "3", "4", "5", "6", "7", "8", "9", "10", "As", "Vale", "Kız", "Papaz" };
            foreach (var sembol in kartsembol)
            {
                foreach (var sayi in kartsayi)
                {
                    toplamdeste.Add(sembol + sayi);
                }
            }
        }
        public void KartKarma()
        {
            for (int i = 1; i <= gun; i++)
            {
                this.toplamdeste = toplamdeste.OrderBy(a => System.Guid.NewGuid()).ToList();
            }
        }
        public void OrtayaKartKoy() //başlangıçta masaya 4 kart koyma 
        {
            for (int i = 0; i < 4; i++)
            {
                masa.Add(toplamdeste[0]);
                toplamdeste.Remove(toplamdeste[0]);
            }
        }
        public void KartDagıt() // oyuncuların ellerine kart verme 
        {
            for (int i = 0; i < 4; i++)
            {
                oyuncu1eli.Add(toplamdeste[0]);
                cekilenkartlar.Add(toplamdeste[0]);
                toplamdeste.Remove(toplamdeste[0]);
            }
            for (int i = 0; i < 4; i++)
            {
                oyuncu2eli.Add(toplamdeste[0]);
                cekilenkartlar.Add(toplamdeste[0]);
                toplamdeste.Remove(toplamdeste[0]);
            }
        }
        public void SonKartAdi()
        {
            if (masa.Count != 0)
            {
                Console.WriteLine($"Masadaki son kart : **{masa.Last()}**  Masadaki toplam kart sayısı : {masa.Count}");
            }
            else if (masa.Count == 0)
            {
                Console.WriteLine("**Masada kart yok** ");
            }
        }
        public void KartAt(List<string> liste, string isim)  //Seçime bağlı kart atma
        {
            while (true)
            {
                if (liste.Count == 4)
                {
                    Console.WriteLine($"Oyuncu **{isim}** ortaya atacağınız kartı seçin: | 1 |   | 2 |   | 3 |    | 4 |)" +
                        $"\n1= {liste[0]} / 2= {liste[1]} / 3= {liste[2]} / 4= {liste[3]}");
                    string atilankart = Console.ReadLine();
                    if (atilankart == "1")
                    {
                        masa.Add(liste[0]);
                        liste.Remove(liste[0]);
                        break;
                    }
                    if (atilankart == "2")
                    {
                        masa.Add(liste[1]);
                        liste.Remove(liste[1]);
                        break;
                    }
                    if (atilankart == "3")
                    {
                        masa.Add(liste[2]);
                        oyuncu1eli.Remove(liste[2]);
                        break;
                    }
                    if (atilankart == "4")
                    {
                        masa.Add(liste[3]);
                        liste.Remove(liste[3]);
                        break;
                    }
                }
                if (liste.Count == 3)
                {
                    Console.WriteLine($"Oyuncu **{isim}** ortaya atacağınız kartı seçin: | 1 |   | 2 |   | 3 |    )" +
                        $"\n1={liste[0]} 2={liste[1]} 3={liste[2]}");
                    string atilankart = Console.ReadLine();
                    if (atilankart == "1")
                    {
                        masa.Add(liste[0]);
                        liste.Remove(liste[0]);
                        break;
                    }
                    if (atilankart == "2")
                    {
                        masa.Add(liste[1]);
                        liste.Remove(liste[1]);
                        break;
                    }
                    if (atilankart == "3")
                    {
                        masa.Add(liste[2]);
                        liste.Remove(liste[2]);
                        break;
                    }

                }
                if (liste.Count == 2)
                {
                    Console.WriteLine($"Oyuncu **{isim}** ortaya atacağınız kartı seçin: | 1 |   | 2 |)\n1={liste[0]} 2={liste[1]}");
                    string atilankart = Console.ReadLine();
                    if (atilankart == "1")
                    {
                        masa.Add(liste[0]);
                        liste.Remove(liste[0]);
                        break;
                    }
                    if (atilankart == "2")
                    {
                        masa.Add(liste[1]);
                        liste.Remove(liste[1]);
                        break;
                    }
                }
                if (liste.Count == 1)
                {
                    Console.WriteLine($"Oyuncu **{isim}** ortaya atacağınız kartı seçin: | 1 |)\n1={liste[0]}");
                    string atilankart = Console.ReadLine();
                    if (atilankart == "1")
                    {
                        masa.Add(liste[0]);
                        liste.Remove(liste[0]);
                        break;
                    }
                }
            }
        }
        public void KartTopla(List<string> liste, int oyuncu, List<int> pisti) //Kart toplama ve pişti bulma 
        {
            if (masa.Count == 2)
            {
                if (masa.Last()[1..] == masa[^2][1..])
                {
                    if (masa.Last()[1..] == "Vale")
                    {
                        sonalankontrol.Add(oyuncu);
                        Console.WriteLine("PİŞTİİİİİİİİ");
                        liste.AddRange(masa);
                        masa.Clear();                        
                        pisti.Add(oyuncu);
                        pisti.Add(oyuncu);
                    }
                    else if (masa.Last()[1..] != "Vale")
                    {
                        sonalankontrol.Add(oyuncu);
                        Console.WriteLine("PİŞTİİİİİİİİ");
                        liste.AddRange(masa);
                        masa.Clear();                        
                        pisti.Add(oyuncu);                       
                    }
                }
                else
                {
                    if (masa.Last()[1..] == "Vale")
                    {
                        sonalankontrol.Add(oyuncu);
                        liste.AddRange(masa);
                        masa.Clear();                                               
                    }
                }
            }
            else if (masa.Count > 2)
            {
                
                if (masa.Last()[1..] == masa[^2][1..])
                {
                    sonalankontrol.Add(oyuncu);
                    liste.AddRange(masa);
                    masa.Clear();                   
                }
                else if (masa.Last()[1..] == "Vale")
                {
                    sonalankontrol.Add(oyuncu);
                    liste.AddRange(masa);
                    masa.Clear();

                }
            }
        }         
        public void MasadaKalanKartlar() // oyun bittiğinde son alan oyuncuya masadaki kartları verme 
        {
            if (sonalankontrol.Last() == 1)
            {
                toplanan1.AddRange(masa);
                masa.Clear();
                Console.WriteLine($"Masada kalan kartlar {sonalankontrol.Last()}. oyuncuya verildi. ");
                Console.WriteLine("\nOYUN BİTTİ\n");
            }
            else if (sonalankontrol.Last() == 2)
            {
                toplanan2.AddRange(masa);
                masa.Clear();
                Console.WriteLine($"Masada kalan kartlar {sonalankontrol.Last()}. oyuncuya verildi. ");
                Console.WriteLine("\nOYUN BİTTİ\n");
            }
        }
        public static int PuanHesaplama(List<string> liste, int puan)   // piştiler harici puan hesaplama 
        {
            foreach (var kart in liste)
            {
                if (kart == "♦10")
                {
                    puan += 3;
                }
                if (kart == "♣2")
                {
                    puan += 2;
                }
                if (kart[1..] == "As")
                {
                    puan += 1;
                }
                if (kart[1..] == "Vale")
                {
                    puan += 1;
                }
            }
            if (liste.Count > 52 - liste.Count)
            {
                puan += 3;
            }
            return puan;
        }
        public void Sonuc()
        {
            puan1 += PuanHesaplama(toplanan1, puan1);
            puan2 += PuanHesaplama(toplanan2, puan2);
            puan1 += (pistisayi1.Count) * 10;
            puan2 += (pistisayi2.Count) * 10;
            if (puan1 > puan2)
            {
                Console.WriteLine($"1. OYUNCU {oyuncu1adi} KAZANDI !! PUANI = {puan1}");

            }
            else if (puan1 < puan2)
            {
                Console.WriteLine($"2. OYUNCU {oyuncu2adi} KAZANDI !! PUANI = {puan2}");

            }
            else
            {
                Console.WriteLine($"BERABERLİK !! OYUNCU PUANLARI = {puan2}");

            }
        } // Kazanan belirleme
        public void KartGecmisi()
        {
            Console.WriteLine("\n*********ÇEKİLEN KART GEÇMİŞİ********\n");
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine($"1. oyuncu {oyuncu1adi}'ın {i + 1}. dağıtımda aldığı kartlar");
                cekilenkartlar.GetRange(2 * i * 4, 4).ForEach(Console.WriteLine);
                Console.WriteLine($"2. oyuncu {oyuncu2adi}'ın {i + 1}. dağıtımda aldığı kartlar");
                cekilenkartlar.GetRange((2 * i + 1) * 4, 4).ForEach(Console.WriteLine);
            }
            Console.WriteLine("\n*********1. OYUNCUNUN TOPLADIĞI KARTLAR********");
            Console.Write(string.Join("-", toplanan1),"\n");
            Console.WriteLine("\n*********2. OYUNCUNUN TOPLADIĞI KARTLAR********");
            Console.Write(string.Join("-", toplanan2),"\n");

        }
        public void Temizleme()  // oyun içindeyken tekrardan oyuna başlanması durumunda değer temizliği
        {
            puan1 = 0;
            puan2 = 0;
            pistisayi1.Clear();
            pistisayi2.Clear();
            sonalankontrol.Clear();
            toplamdeste.Clear();
            oyuncu1eli.Clear();
            oyuncu2eli.Clear();
            toplanan1.Clear();
            toplanan2.Clear();
            cekilenkartlar.Clear();
        }
    }
}
