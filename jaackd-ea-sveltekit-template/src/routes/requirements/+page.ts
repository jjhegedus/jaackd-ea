import type { PageLoad } from './$types';
import { requirements } from './requirements';

export const load = (async () => {
    return {
		requirements
		};
}) satisfies PageLoad;