# Achievement Study Bot

This repository hosts a Discord study bot developed specifically for the educational purposes of the PV178 course at the Faculty of Informatics, Masaryk University.

The primary function of this Discord bot is to facilitate students in requesting educational achievements and enable teachers to acknowledge these achievements through a Discord server interface.

### Main Functionality
- **Teacher Registration**: The bot administrator can register new teachers.
- **Student Registration**: Students can sign up using specific commands.
- **Teacher-Student Association**: Teachers can select which students they teach.
- **Achievement Requests**: Students can request achievements from their teachers.
- **Achievement Approval**: Teachers granting achievements also award points, potentially elevating the student's rank.

## Project Images

Below are images demonstrating the user interface for requesting and approving achievements via the Discord UI:

### Requesting Achievements
![Requesting achievements using the Discord UI](https://github.com/Gilderko/PV178StudyBot/blob/master/Images/imageStudy2.png?raw=true "Requesting Achievements")

### Approving Achievements
![Approving achievements using the Discord UI](https://github.com/Gilderko/PV178StudyBot/blob/master/Images/imageStudy1.png?raw=true "Approving Achievements")

## Deployment
To deploy the Achievement Study Bot, you can use docker compose. 
To get quickly started with the Achievement Study Bot, you can use docker compose. First, create a `.env` file in the `deploy` folder, where you specify the database password and discord token. The file should look like this:
```
DB_PASSWORD=QuiteALongPasswordHere
DISCORD_TOKEN=YourDiscordToken
```
With the `.env` file created, simply call:
```
docker compose up
```

### Discord Token
### Database migration
The database is migrated using a SQL script when the db container is initialized. If you would need to refresh the SQL script, you can generate one.
First, run a fresh mysql container:
```
docker run --rm -p 3306:3306 -e MYSQL_DATABASE=PV178BotDB -e MYSQL_USER=pv178studybot -e MYSQL_PASSWORD=AtEmPpassW0rddt0tHebOT -e MYSQL_RANDOM_ROOT_PASSWORD=true mysql:5.6
```
Then, execute the following commands.
```
cd PV178StudyBot
$env:PV178StudyBot_ConnectionString="Server=localhost;Database=PV178BotDB;Uid=pv178studybot;Pwd=AtEmPpassW0rddt0tHebOT;"
dotnet ef migrations script
```
The last command shall output the script, which you can copy to the `deploy\db_init.sql` file.