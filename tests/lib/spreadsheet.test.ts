import { expect, it } from 'vitest';
import * as XLSX from 'xlsx';

it.each(['xlsx', 'xls', 'ods'] as const)('preserves stationery names when reading %s workbooks', (bookType) => {
  const rows = [['Product'], ['A4 exercise book'], ['Blue pen']];
  const book = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(book, XLSX.utils.aoa_to_sheet(rows), 'Stationery');
  const bytes = XLSX.write(book, { type: 'array', bookType });
  const imported = XLSX.read(bytes, { type: 'array' });
  expect(XLSX.utils.sheet_to_json(imported.Sheets[imported.SheetNames[0]], { header: 1 })).toEqual(rows);
});
