import { requirements } from '../requirements.js';
import { error } from '@sveltejs/kit';


export function load({ params }) {
	const requirement = requirements.find((requirements) => requirements.id === params.slug);

  
	if (!requirement) throw error(404);

	return {
		requirement: requirement
	};
}