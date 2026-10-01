import { expect, it } from 'vitest';
import { createElement } from 'react';
import { renderToStaticMarkup } from 'react-dom/server';
import { Button, ThemeProvider } from 'flowbite-react';

it('renders Flowbite components with custom themes after the deepmerge security upgrade', () => {
  const markup = renderToStaticMarkup(createElement(ThemeProvider, {
    theme: { button: { base: 'custom-theme-marker' } },
  }, createElement(Button, null, 'Checkout')));
  expect(markup).toContain('custom-theme-marker');
  expect(markup).toContain('Checkout');
  expect(markup).toContain('<button');
});
