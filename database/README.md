  # Adatbázis Kapcsolatok és Azure SQL Database Serviceről

Ez a dokumentum bemutatja a relációs adatbázis táblái közötti kapcsolatok strukturális felépítését, valamint néhány fontos információt az Azure SQL Database szolgáltatásról.

---

## Adatbázis Kapcsolatok

Az adatbázis több táblából áll, melyek közötti kapcsolatokat az alábbiak szerint definiáltuk:

- **Universities (Egyetemek)**
  - **Kapcsolat:**  
    - **Majors:** Az `majors` (szakok) tábla `UniversityId` oszlopa idegen kulcsként hivatkozik az `universities` tábla `Id` mezőjére.
    
- **Majors (Szakok)**
  - **Kapcsolatok:**
    - **Universities:** Egy szak egy egyetemhez tartozhat (opcionális).
    - **Users:** A `users` tábla `MajorId` oszlopa hivatkozik az `majors` tábla `Id` mezőjére.
    - **Subjects:** A `subjects` tábla `MajorId` oszlopa idegen kulcsként mutat az `majors` tábla `Id` mezőjére.
    
- **Users (Felhasználók)**
  - **Kapcsolatok:**
    - **Majors:** A `users` tábla opcionálisan kötődik a `majors` táblához a `MajorId` oszlopon keresztül.
    - **Generated Timetables:** A `generatedtimetables` tábla `UserId` oszlopa hivatkozik a `users` tábla `Id` mezőjére.
    - **Refresh Tokens:** A `refreshtokens` tábla `UserId` oszlopa idegen kulcsként mutat a `users` táblára.
    - **Audit Log:** Az `auditlog` tábla `ChangedBy` oszlopa a `users` tábla `Id` mezőjére utal.
    - **Completed Subjects:** A `completedsubjects` tábla is két oszlopával (`UserId`) kapcsolódik a `users` táblához.
    
- **Generated Timetables (Létrehozott Menetrendek)**
  - **Kapcsolat:**  
    - **Users:** A `generatedtimetables` tábla `UserId` oszlopa hivatkozik a `users` tábla `Id` mezőjére.
    - **Subjects:** A `subjects` tábla opcionálisan kapcsolódik a `generatedtimetables` táblához a `GeneratedTimetableId` oszlopon keresztül.
    - **Class Schedules:** A `classschedules` tábla opcionálisan tartalmazza a `GeneratedTimetableId` hivatkozást.
    
- **Subjects (Tantárgyak)**
  - **Kapcsolatok:**
    - **Majors:** A `subjects` tábla `MajorId` oszlopa hivatkozik a `majors` tábla `Id` mezőjére.
    - **Generated Timetables:** A `subjects` tábla opcionálisan kapcsolódik a `generatedtimetables` táblához a `GeneratedTimetableId` oszlopon keresztül.
    - **Subject Prerequisites:** A `SubjectPrerequisites` tábla két oszlopa (`PrerequisiteId` és `SubjectId`) hivatkozik a `subjects` táblára. Egy tantárgyhoz több előfeltétel tartozhat.
    - **Completed Subjects:** A `completedsubjects` tábla `SubjectId` oszlopa hivatkozik a `subjects` tábla `Id` mezőjére.
    - **Class Schedules:** A `classschedules` tábla `SubjectId` oszlopa a `subjects` tábla `Id` mezőjére hivatkozik.
    
- **SubjectPrerequisites (Tantárgy Előfeltételek)**
  - **Kapcsolatok:**  
    - A két oszlop (`PrerequisiteId` és `SubjectId`) azonos `subjects` tábla rekordokra mutat, meghatározva, mely tantárgyak melyek előfeltételei.
    
- **Completed Subjects (Teljesített Tantárgyak)**
  - **Kapcsolatok:**  
    - **Users:** A `completedsubjects` tábla `UserId` oszlopa a `users` tábla rekordjaira hivatkozik.
    - **Subjects:** A `completedsubjects` tábla `SubjectId` oszlopa a `subjects` tábla rekordjaira utal.
    
- **Class Schedules (Órarendek)**
  - **Kapcsolatok:**  
    - **Subjects:** A `classschedules` tábla `SubjectId` oszlopa hivatkozik a `subjects` tábla `Id` mezőjére.
    - **Generated Timetables:** Opcionális hivatkozás a `generatedtimetables` táblára a `GeneratedTimetableId` oszlopon keresztül.
    
- **Refresh Tokens (Frissítési Tokenek)**
  - **Kapcsolat:**  
    - A `refreshtokens` tábla `UserId` oszlopa a `users` tábla `Id` mezőjére hivatkozik.
    
- **Audit Log**
  - **Kapcsolat:**  
    - Az `auditlog` tábla `ChangedBy` oszlopa idegen kulcsként a `users` tábla `Id` mezőjére hivatkozik.

---

## Azure SQL Database Service

Az Azure SQL Database egy felhőalapú relációs adatbázis szolgáltatás, amely nagyfokú rendelkezésre állást, skálázhatóságot és biztonságot biztosít az alkalmazásaink számára. Az alábbi JSON részlet néhány fontos tulajdonságát mutatja be:

- **Sku (Szolgáltatáscsomag):**  
  A `GP_S_Gen5` csomag a GeneralPurpose kategóriába tartozik, Gen5 családba sorolva, és 2 vCore kapacitással rendelkezik. Ez a csomag optimális ár-érték arányt kínál általános célú alkalmazások számára.

- **Collation és Egyéb Tulajdonságok:**  
  A kollekció `Hungarian_CI_AI`, ami a magyar nyelv specifikus összehasonlítási szabályait használja, biztosítva a karakterek megfelelő rendezését és keresését.  
  A szolgáltatás támogatja az auto-pause funkciót, amely 60 perc után automatikusan "szünetelteti" a szolgáltatást a használat csökkentése érdekében (ami költséghatékonyságot segít).

- **Egyéb Információk:**  
  - A szolgáltatás **serverless** működésű, ami azt jelenti, hogy a skálázás automatikusan történik, és csak a ténylegesen használt erőforrásokért kell fizetni.  
  - A jelenlegi szolgáltatási állapot **Online**, és a backupok helyileg redundáns tárolással készülnek el.  
  - A rendszer támogatja az automatikus felfüggesztést és az alacsonyabb minimális kapacitást (minCapacity 0.5), így rugalmas erőforráskezelést biztosít.

Ez az adatbázis szolgáltatás ideális választás lehet olyan alkalmazások számára, amelyek rugalmas, skálázható és menedzselt adatbázis környezetet igényelnek.

---

Ez a dokumentum áttekintést nyújt az adatbázis struktúrájáról és a főbb kapcsolatairól, továbbá ismertet néhány kulcsfontosságú jellemzőt az Azure SQL Database szolgáltatásról, amely a magas rendelkezésre állás, teljesítmény és egyszerű menedzsment biztosítása mellett kiváló megoldás a modern felhőalapú alkalmazások számára.
