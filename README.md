
# DONE



# TODO

## ASP.NET Core Web API

* NET Compiler platform (Roslyn) Diagnostic Analyzer [3-7]
	egyszerű analyzer, pl. property név konvenciók ellenőrzése 3


* verziókezelt API [7-10]
  * nem HTTP header (pl. URL szegmens) alapján 7

* Szerver oldali autentikáció [7-18]
  * ASP.NET Core Identity middleware-rel, süti alapú - csak webes kliens esetén! 7


## Kommunikáció, hálózatkezelés

## Entity Framework Core

* birtokolt típus (owned type) használata [3]
* DbContext health check a Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore NuGet csomag használatával 


## .NET Core részfunkciók alkalmazása

* kifejezésfa (ExpressionTree) értelmezése és manipulálása **\[5-20\]**
    * pl. szűrés dinamikusan, paraméterből érkező property neve alapján (pl. `o => o.Prop == x`) **5**
    * pl. keresés kapcsolódó kollekcióban dinamikusan (pl. `o => o.Coll.Any(e => e.Prop == x)`) **10**
    * saját LINQ provider **20**
    * 
* unit tesztek készítése [5-12]
  * minimum 10 függvényhez 5
  * a unit tesztekben a mock objektumok injektálása +3
  * EF Core memória-adatbázis használata teszteléshez +4
  * 
* Object mapper (pl. [AutoMapper](http://automapper.org/), [QueryMutator](https://github.com/yugabe/QueryMutator)) használata DTO-k létrehozására **\[5\]**