/**
 * 
 * @param {string} input 
 * @param  {string[]} cmdName
 * @returns {boolean} if the input corresponds to one of the given command names
 */
exports.assertCommandName = (input, ...cmdNames) => cmdNames.includes(input);