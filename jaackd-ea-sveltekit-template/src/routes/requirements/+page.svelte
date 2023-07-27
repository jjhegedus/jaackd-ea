<script>
	export let data;
</script>

<h1>blog</h1>

<ul>
	{#each data.requirements as { title, id }}
		<li><a href="/requirements/{id}">{title}</a></li>
	{/each}
</ul>