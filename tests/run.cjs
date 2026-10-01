const { spawnSync } = require('node:child_process');
const path = require('node:path');

// Run both suites even if one fails, so CI reports every regression.
const root = path.join(__dirname, '..');
let failed = false;
for (const args of [
  ['--test', 'tests/commerce.test.cjs', 'tests/security.test.cjs'],
  ['node_modules/vitest/vitest.mjs', 'run'],
]) {
  const result = spawnSync(process.execPath, args, { cwd: root, stdio: 'inherit' });
  if (result.error || result.status !== 0) failed = true;
}
process.exitCode = failed ? 1 : 0;
