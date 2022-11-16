const Database = require('better-sqlite3');
const db = new Database('./HorseDb.db', { verbose: console.log });

let horseSql;
let raceSql;
let horseInRaceSql;

db.prepare('PRAGMA foreign_keys = ON').run();

horseSql = `CREATE TABLE horses(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name VARCHAR(255),
    color VARCHAR(128),
    UNIQUE(name));`;

raceSql = `CREATE TABLE races(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    location VARCHAR(255),
    datetime DATETIME);`;

horseInRaceSql = `CREATE TABLE horseinrace(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    horseid INTEGER, 
    raceid INTEGER, 
    place INTEGER,
    bet DOUBLE,

    CONSTRAINT fk_races
    FOREIGN KEY (raceid)
    REFERENCES races(id)
    ON DELETE CASCADE

    CONSTRAINT fk_horses
    FOREIGN KEY (horseid) 
    REFERENCES horses (id) 
    ON DELETE CASCADE)
    `;

alterHorsesInRaceSql = `
    ALTER TABLE horseinrace
    ADD `

// db.run('DROP TABLE horses');
// db.run('DROP TABLE race');
// db.run('DROP TABLE horseinrace');

// db.run(horseSql);
// db.run(raceSql);
// db.run(horseInRaceSql);
db.prepare('DROP TABLE IF EXISTS horseinrace').run();
db.prepare('DROP TABLE IF EXISTS races').run();
db.prepare('DROP TABLE IF EXISTS horses').run();
db.prepare(horseSql).run();
db.prepare(raceSql).run();
db.prepare(horseInRaceSql).run();

// db.prepare(alterHorsesInRaceSql).run();

// db.prepare('DROP TABLE IF EXISTS race').run().then(db.prepare(raceSql).run())
// db.prepare('DROP TABLE IF EXISTS horses').run().then(db.prepare(horseInRaceSql).run())
/// create dables

// db.run(horseSql);
// db.run(raceSql);
// db.run(horseInRaceSql);