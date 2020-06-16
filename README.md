
# DONE

* Automapper: 5 pont
* ASP.NET Core Identity middleware-rel, süti alapú: 7 pont
* szűrés dinamikusan, paraméterből érkező property neve alapján (pl.  o => o.Prop == x ) 5 pont
* verziokezelt API HTTP header: 10 pont
* diagnosztika beépített vagy külső komponens segítségével 9 pont


# TODO

## ASP.NET Core Web API

* NET Compiler platform (Roslyn) Diagnostic Analyzer [3-7]
	egyszerű analyzer, pl. property név konvenciók ellenőrzése 3


* verziókezelt API [7-10]
  * nem HTTP header (pl. URL szegmens) alapján 7

* Szerver oldali autentikáció [7-18]
  * ASP.NET Core Identity middleware-rel, süti alapú - csak webes kliens esetén! 7
Publikálás docker konténerbe és futtatás konténerből [7]

## Kommunikáció, hálózatkezelés

## Entity Framework Core

* birtokolt típus (owned type) használata [3]
* DbContext health check a Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore NuGet csomag használatával 
* újrapróbálkozás beállítása tranziens adatbázishibák (pl. connection timeout) ellen [2]
* birtokolt típus (owned type) használata [3]
* adatbetöltés (seeding) migráció segítségével (HasData) [3]
* DbContext health check végpont publikálása a Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore NuGet csomag használatával [3]
* 
## .NET Core részfunkciók alkalmazása

* unit tesztek készítése [7-14]
    * minimum 10 függvényhez 7
    * a unit tesztekben a mock objektumok injektálása +3
    * EF Core memória-adatbázis vagy sqlite (vagy in-memory sqlite) használata teszteléshez +4
 

* platformfüggetlen kódbázisú szerveralkalmazás készítése és bemutatása legalább 2 operációs rendszeren az alábbiak közül: Windows, Linux, Mac, ARM alapú OS (Raspberry Pi). [7]

