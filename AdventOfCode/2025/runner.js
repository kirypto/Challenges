const [_, __, day, part, file] = process.argv;
let $$$ = require(`./day${day}-part${part}.wat.js`);
let input = require('fs').readFileSync(file, 'utf8');
console.log($$$(input));

