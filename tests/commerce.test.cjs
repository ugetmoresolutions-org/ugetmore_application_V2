const test = require('node:test');
const assert = require('node:assert/strict');
const { loadSource, storage } = require('./helpers.cjs');

const prices = [{ brandingCode: 'PRINT', brandingMethod: 'Print', data: [
  { minQuantity: 1, maxQuantity: 49, setup: 100, price: 10 },
  { minQuantity: 50, maxQuantity: -1, setup: 100, price: 8 },
] }];

test('branding tier changes at the inclusive quantity boundary', () => {
  const { getBrandingPrice } = loadSource('components/shop/branding/brandingPricing.ts');
  assert.equal(getBrandingPrice('PRINT', 49, prices).price, 10);
  assert.equal(getBrandingPrice('PRINT', 50, prices).price, 8);
  assert.equal(getBrandingPrice('PRINT', 10000, prices).price, 8);
});

test('multiple colors share one setup fee and include a requested design fee', () => {
  const { calculateTotalOrderCost } = loadSource('components/shop/branding/brandingPricing.ts');
  const colors = [
    { colorCode: 'red', quantity: 10, unitPrice: 20 },
    { colorCode: 'blue', quantity: 10, unitPrice: 20 },
  ];
  const positions = [{ selected: true, code: 'front', appliedToColors: ['red', 'blue'],
    selectedMethod: { brandingCode: 'PRINT' } }];
  const total = calculateTotalOrderCost(colors, positions, [{ artworkOption: 'design' }], {}, prices);
  assert.equal(total.basePrice, 400);
  assert.equal(total.brandingCost, 200);
  assert.equal(total.setupFees, 100);
  assert.equal(total.designFees, 250);
  assert.equal(total.grandTotal, 950);
});

test('empty guest cart makes no backend requests', async () => {
  let calls = 0;
  const { CartSyncService } = loadSource('utils/cartSync.ts', {
    '@/endpoints/rest-api/cart': { CART_API: { GET_USER_CART() { calls++; } } },
  }, { window: {}, localStorage: storage() });
  const result = await CartSyncService.syncLocalCartToUserAccount(42);
  assert.equal(result.success, true);
  assert.equal(result.mergedItemsCount, 0);
  assert.equal(calls, 0);
});

test('failed guest cart upload retains the item for retry', async () => {
  const item = { id: 'item-a', product: { fullCode: 'SKU-A', productName: 'Test' }, quantity: 2, price: 20 };
  const localStorage = storage({ cart: JSON.stringify([item]) });
  const { CartSyncService } = loadSource('utils/cartSync.ts', {
    '@/endpoints/rest-api/cart': { CART_API: {
      GET_USER_CART: async () => ({ data: { items: [] } }),
      ADD_CART_ITEM: async () => { throw new Error('Service unavailable'); },
    } },
  }, { window: { dispatchEvent() {} }, Event: class {}, localStorage });
  const result = await CartSyncService.syncLocalCartToUserAccount(42);
  assert.equal(result.success, false);
  assert.equal(result.mergedItemsCount, 0);
  assert.deepEqual(JSON.parse(localStorage.getItem('cart')), [item]);
});
