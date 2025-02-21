import { API_BASE } from './constants'
const _bearer = () => `Bearer ${localStorage.getItem('CA_Token')}`

const buildUrl = (endpoint, urlParams) => {
	let full_url = `${API_BASE}/${endpoint}`
	if (urlParams.length) {
		full_url = `${full_url}/${urlParams.join('/')}`
	}
	return full_url
}

export const api_get = async ({ endpoint, urlParams = [] }) => {
	const url = buildUrl(endpoint, urlParams)
	const response = await fetch(url, {
		method: 'GET',
		headers: {
			Authorization: _bearer()
		}
	})
	return handleResponse(response)
}

export const api_post_form_data = async ({ endpoint, form, urlParams = [] }) => {
	const response = await fetch(buildUrl(endpoint, urlParams), {
		method: 'POST',
		headers: {
			Authorization: _bearer()
		},
		body: form
	})
	return handleResponse(response)
}

export const api_post = async ({ endpoint, body, urlParams = [] }) => {
	const response = await fetch(buildUrl(endpoint, urlParams), {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
			Authorization: _bearer()
		},
		body: JSON.stringify(body || {}),
		json: true
	})
	return handleResponse(response)
}

export const api_delete = async ({ endpoint, urlParams = [] }) => {
	const response = await fetch(buildUrl(endpoint, urlParams), {
		method: 'DELETE',
		headers: {
			Authorization: _bearer()
		}
	})
	return handleResponse(response)
}
