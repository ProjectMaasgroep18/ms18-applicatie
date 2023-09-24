_In dit document wordt informatie gedeeld die benodigd is 
voor het effectief samenwerken van de verschillende teams.
Lees deze informatie dan ook goed door!_

# GIT Repository

De git repository voor dit project is al volgt gestructureerd:

![](https://raw.githubusercontent.com/ProjectMaasgroep18/ms18-applicatie/main/git_structuur.jpg)

In de structuur zijn de volgende onderdelen aanwezig:
1. `master` branch _Op deze branch wordt de productie versie van de applicatie bewaard. Het is mogelijk om pull requests te maken naar de master branch, deze zijn echter alleen te accepteren door de repository admins_
2. `develop` branch _Op deze branch staat de laatste test/acceptatie versie. Er wordt van de teams verwacht dat afgemaakte functionaliteiten hier naartoe worden gemerged d.m.v. een pull request._
3. `team-x/xxxx` branch _Deze branch is vergelijkbaar met de develop branch alleen staat op de branch de totale feature waar een team mee bezig is. Zodra een team dus helemaal klaar is met een feature staat deze in zijn totaal op deze branch en kan er vanaf deze branch een pull request richting develop gedaan worden_
4. `feat/xxxx` branch _Dit zijn de persoonlijke branches. Hier werken team leaden aan kleine onderdelen van de totale team feature. Net zoals bij de bovengenoemde branches worden de aanpassingen hier naar de bovenliggende branch (team-x/xxxx) gemerged via een pull request._

## Regel voor mergen

Binnen de repository zijn een aantal regel gedefineerd om te zorgen dat de samenwerking goed loopt en dat iedereen de kans krijgt om de bijdragen van andere te controleren. **Zodra een functionaliteit beschikbaar wordt gemaakt op de develop branch zijn wij er allemaal verantwoordelijk voor, neem dus de tijd voor het reviewen van pull requests.**

De regels zijn als volgt:

1. `develop` naar `master`: Moet goedgekeurd worden door de repository owners
2. `team-x/xxxx` naar `develop`: Minimaal 4 approvements op de pull request van andere personen en minimaal 1 _Codeowner_ moet een approvment geven op de code waar hij/zij owner van is.
3. `feat/xxxx` naar `team-x/xxxx`: Minimaal 2 approvements op de pull request van andere personen en minimaal 1 _Codeowner_ moet een approvment geven op de code waar hij/zij owner van is.

## Codeowners

Binnen het project zijn _Codeowners_ gedefineerd. Dit houd in dat specifieke personen owner zijn van specifieke delen van de code. 

Hierien maken we onderscheid in code van de teams en code van het totaal.

Voor alle bestanden in een map `/team-a` is _Team A_ de codeowner. Dit betekend dat wanneer hier een aanpassing gedaan wordt minimaal 1 _Team A_ lid deze aanpassing moet goedkeuren. Hiermee voorkomen we dat teams elkaars functionliteiten kunnen aanpassen zonder dat het team welke dit origineel heeft opgezet hiervan weet.

De positie van de `/team-a` map niet uit. Dat betekend het volgende `/ms18-applicatie/team-a/BESCHERMD` 
maar ook `/ms18-applicatie/Controllers/team-a/BESCHERMD`. Zorg dus dat als je een view,controller of model aanmaakt deze altijd in de juiste team map zit. 

Alle code die niet in een team map zit wordt beheerd door de integratie groep en deze moet dus goedkeuring geven op aanpassingen hierbinnen.