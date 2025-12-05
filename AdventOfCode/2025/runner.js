const [_, __, day, file] = process.argv;
let $$$ = require(`./day${day}.wat.js`);
let input = require('fs').readFileSync(file, 'utf8');
console.log($$$(input));

