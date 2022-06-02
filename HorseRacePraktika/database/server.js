const express = require('express');
const cors = require('cors');
const app = express();

const PORT = 9000;

app.use(express.json());
app.use(cors())


////// rest-api //////

// horse
app.post('/api/horse', (req, res) => {

    const horse = req.body;

    AddHorse(horse['name'], horse['color'])
    res.send(horse);
})

app.get('/api/horse', (req, res) => {

    res.setHeader("Content-Type", "application/json");
    res.json(GetAllHorses());
})

app.delete('/api/horse/:id', (req, res) => {
    DeleteHorse(req.params.id)
    res.json(GetAllHorses());
})

// race

app.post('/api/race',async (req, res) => {


    const raceData = req.body;

    console.log(raceData)

    let dateRaw = raceData['date'];
    let timeRaw = raceData['time'];
    let horsesInRace = raceData['horsesInRace'];
    let selectedHorse = raceData['selectedHorse'];
    let bet = raceData['bet'];

    let dateinfo = dateRaw.split('/');
    let timeinfo = timeRaw.split(':')

    var date = new Date(`${dateinfo[2]}-${dateinfo[1]}-${dateinfo[0]}T${timeinfo[0]}:${timeinfo[1]}`);

    let raceid = await AddRace(raceData['location'], date.toISOString())['lastInsertRowid'];
    
    if (raceid) {
        
        let i = 1;
        

        shuffle(horsesInRace);
        
        horsesInRace.forEach(horse => {
            let betOn = null;

            if (horse['id'] == selectedHorse['id']){
                betOn = bet
            }
            
            if(AddHorseToRace(horse['id'], raceid, i, betOn)) {
                i++;
            }
        
        });
        
        // res.setHeader("Content-Type", "application/text");
        res.send({id: raceid});
       
    }

})

app.get('/api/race/:id', (req, res) => {
    res.setHeader("Content-Type", "application/json");
    res.json(GetRace(req.params.id));
})

app.get('/api/race', (req, res) => {
    res.setHeader("Content-Type", "application/json");
    res.json(GetAllRaces());
})

app.listen(PORT, () => console.log(`Server running on port: http://localhost:${PORT}`));

// Horsesinrace

app.get('/api/race/:id', (req, res) => {
    res.setHeader("Content-Type", "application/json");
    res.json(GetRace(req.params.id));
})
////// server //////

const Database = require('better-sqlite3');
const db = new Database('./database/HorseDb.db', { verbose: console.log });

function AddHorse(horseName, horseColor) {

    const stmt = db.prepare('INSERT INTO horses (name, color) VALUES (?, ?)');
    stmt.run(horseName, horseColor);

}

function GetAllHorses() {

    const stmt = db.prepare('SELECT * FROM horses');
    const horsesFromDb = stmt.all();

    return horsesFromDb;

}

async function DeleteHorse(id) {

    const stmt = db.prepare('SELECT * FROM horseinrace WHERE horseid = (?)');
    
    let horseinRace = await stmt.get(id)
    
    if (horseinRace !== undefined) {
            console.log("0000000000000000000")
            const stmtDeleteRace = db.prepare('DELETE FROM races WHERE id = (?)');
            stmtDeleteRace.run(horseinRace['raceid']);
    }

    const stmtDeleteHorse = db.prepare('DELETE FROM horses WHERE id = (?)');
    
    stmtDeleteHorse.run(id);
    
}

function AddRace(location, date) {

    console.log(location)
    console.log(date)
    const stmt = db.prepare('INSERT INTO races (location, datetime) VALUES (?, ?)');
    return stmt.run(location, date);

}

function AddHorseToRace(horseid, raceid, place, bet) {

    const stmt = db.prepare('INSERT INTO horseinrace (horseid, raceid, place, bet) VALUES (?, ?, ?, ?)');
    return stmt.run(horseid, raceid, place, bet);

}

function GetAllRaces() {

    const stmt = db.prepare('SELECT * FROM races ORDER BY datetime');
    const racesFromDb = stmt.all();

    return racesFromDb;

}

function GetRace(id) {

    const stmt = db.prepare('SELECT * FROM horseinrace LEFT JOIN races ON horseinrace.raceid = races.id LEFT JOIN horses ON horseinrace.horseid = horses.id WHERE races.id = (?) ORDER BY horseinrace.place');
    const raceFromDb = stmt.all(id);
    console.log(raceFromDb);
    return raceFromDb;
}
/// utility

function shuffle(array) {
    let currentIndex = array.length,  randomIndex;
  
    while (currentIndex != 0) {
  
      randomIndex = Math.floor(Math.random() * currentIndex);
      currentIndex--;
  
      [array[currentIndex], array[randomIndex]] = [
        array[randomIndex], array[currentIndex]];
    }
  
    return array;
  }