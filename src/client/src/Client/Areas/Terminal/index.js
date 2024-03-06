/**
 * Beccause an Area is tehnically a group of related Pages, we only want
 * to export components from the pages folder. All else is common to the
 * creation and makeup of the pages, like forms, providers, hooks, and etc.
 */

export * from './pages';
export { TerminalStoreProvider } from './providers';
