# UNIVERZITET U ZENICI  
### POLITEHNIČKI FAKULTET  
**Softversko inženjerstvo**  
**Razvoj mobilnih aplikacija**  

---

# Music Streaming Service  

**Mentori:**  
- Doc. dr. Esad Kadušić  
- V. Asist. Sanid Muhić  

**Studenti:**  
1. Safet Imamović  
2. Hamza Gačić  

**Zenica, 2024.**

---

## Sadržaj  
1. [Opis okruženja i demografski target korištenja](#opis-okruženja-i-demografski-target-korištenja)  
2. [Korisnički zahtjevi](#korisnički-zahtjevi)  
3. [Funkcionalni zahtjevi](#funkcionalni-zahtjevi)  
4. [Prijedlog UI/UX dizajna](#prijedlog-uiux-dizajna)  
5. [Zaključak](#zaključak)  

---

## 1. Opis okruženja i demografski target korištenja  
Aplikacija je namijenjena demografskom targetu koji obuhvata:  
- **Dobna skupina:** 15–35 godina  
- **Korisnici:** Studenti, mladi profesionalci, muzički entuzijasti  
- **Geografska pokrivenost:** Primarno Bosna i Hercegovina, sekundarno ex-Yu regija  

**Opis problema:**  
Postojeći streaming servisi često ne pružaju lokalizirano iskustvo korisnicima iz naše regije. **Music Streaming Service** aplikacija rješava ovaj problem omogućavajući korisnicima da slušaju muziku na jeziku i platformi prilagođenoj njihovim potrebama.  

---

## 2. Korisnički zahtjevi  

### Cilj projekta  
Razviti aplikaciju koja omogućava korisnicima:  
- Slušanje muzike  
- Kreiranje plejlista  
- Personalizaciju interfejsa  
- Preuzimanje pjesama za offline slušanje  

### Svrha projekta  
- Omogućiti jednostavno i efikasno slušanje muzike  
- Pružiti korisnicima intuitivno i prilagodljivo iskustvo  
- Podržati personalizaciju i pristupačnost za širok spektar korisnika  

### Korisnički slučajevi korištenja  
1. **Registracija i kreiranje profila:**  
   Korisnik može kreirati profil putem e-maila i sigurne šifre.  
2. **Login i pregled sadržaja:**  
   Nakon prijave, korisniku je dostupan katalog pjesama, izvođača i albuma.  
3. **Kreiranje plejlista:**  
   Korisnik može organizovati omiljene pjesme u personalizovane liste.  
4. **Offline slušanje:**  
   Preuzimanje pjesama za slušanje bez pristupa internetu.  

---

## 3. Funkcionalni zahtjevi  

### Funkcije aplikacije  
1. **Sistem autentifikacije:**  
   - Registracija i prijava putem e-maila i lozinke.  
   - Podaci se sigurno čuvaju u bazi podataka s enkripcijom lozinki.  

2. **Pregled muzičkog kataloga:**  
   - Pregled kataloga pjesama, izvođača i albuma.  
   - Sistem filtriranja i pretraživanja sadržaja.  

3. **Kreiranje i uređivanje plejlista:**  
   - Kreiranje personalizovanih plejlista.  
   - Dodavanje i uklanjanje pjesama iz plejlista.  

4. **Streaming muzike:**  
   - Reprodukcija muzike putem interneta.  
   - Streaming optimizovan za različite brzine internetske veze.  

5. **Offline režim:**  
   - Preuzimanje pjesama za slušanje bez interneta.  
   - Dostupno samo za registrovane korisnike.  

6. **Personalizacija korisničkog iskustva:**  
   - Prilagodba teme (svijetla ili tamna).  
   - Generisanje preporuka na osnovu korisničkih interakcija.  

7. **Sistem notifikacija:**  
   - Obavijesti o novim pjesmama, albumima i ažuriranjima.  
   - Podešavanje obavijesti u aplikaciji.  

### Tabela funkcionalnih specifikacija  
| ID        | Opis                                 | Komentar                                    |  
|-----------|--------------------------------------|--------------------------------------------|  
| F.1.0.0   | Sistem autentifikacije              | Sigurna registracija i prijava korisnika   |  
| F.2.0.0   | Pregled muzičkog kataloga           | Prikaz svih dostupnih pjesama i izvođača   |  
| F.3.0.0   | Kreiranje plejlista                 | Personalizacija kroz dodavanje muzike      |  
| F.4.0.0   | Streaming muzike                    | Usluga optimizovana za streaming           |  
| F.5.0.0   | Offline režim                       | Slušanje muzike bez internetske veze       |  
| F.6.0.0   | Personalizacija korisničkog iskustva| Prilagodba teme i preporuke                |  
| F.7.0.0   | Sistem notifikacija                 | Obavještavanje o novostima i ažuriranjima  |  

---

## 4. Prijedlog UI/UX dizajna  

**Aplikacije koje rješavaju sličan problem:**  
- Spotify  
- Deezer  
- Tidal  

**Prijedlog dizajna aplikacije:**  
1. **Splash screen:** Animirani logo kompanije.  
2. **Welcome screen:** Carousel sa informacijama o aplikaciji.  
3. **Register/Login screen:** Intuitivne forme sa validacijom podataka.  
4. **Home/Dashboard:** Pregled kategorija, plejlista i muzike.  
5. **Settings screen:** Podešavanje tema i opcija pristupačnosti.  

**Detaljan dizajn dostupan na linku:** [Link do dizajna]  

---

## 5. Zaključak  
Music Streaming Service aplikacija osmišljena je s ciljem pružanja lokaliziranog i prilagodljivog korisničkog iskustva. Projekat integriše inovativne funkcionalnosti kao što su offline režim i personalizacija korisničkog sučelja, čineći ga idealnim rješenjem za muzičke entuzijaste u regiji.  

---