const fs = require('node:fs');
const path = require('node:path');
const vm = require('node:vm');
const ts = require('typescript');

// Execute real TypeScript modules with explicit dependency doubles, never live APIs.
function loadSource(filename, dependencies = {}, globals = {}) {
  const source = fs.readFileSync(path.join(__dirname, '..', filename), 'utf8');
  const { outputText } = ts.transpileModule(source, {
    compilerOptions: { module: ts.ModuleKind.CommonJS, target: ts.ScriptTarget.ES2020 },
  });
  const exports = {};
  const sandbox = {
    exports,
    require(name) {
      if (Object.hasOwn(dependencies, name)) return dependencies[name];
      throw new Error(`Test dependency not provided: ${name}`);
    },
    console: { log() {}, error() {}, warn() {} },
    URL,
    ...globals,
  };
  vm.runInNewContext(outputText, sandbox, { filename, timeout: 5000 });
  return exports;
}

function storage(initial = {}) {
  const data = new Map(Object.entries(initial));
  return {
    getItem: (key) => data.get(key) ?? null,
    setItem: (key, value) => data.set(key, String(value)),
    removeItem: (key) => data.delete(key),
  };
}

module.exports = { loadSource, storage };
