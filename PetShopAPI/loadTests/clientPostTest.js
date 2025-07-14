import { check, sleep } from 'k6';
import http from 'k6/http';

export let options = {
    stages: [
        { duration: '10s', target: 10 },
        { duration: '20s', target: 20 },
        { duration: '10s', target: 0 },
    ],
};

const BASE_URL = 'https://localhost:7001';

export default function () {
    let payload = JSON.stringify({
        nome: `Cliente ${__VU}-${__ITER}`,
        email: `cliente${__VU}-${__ITER}@teste.com`,
        telefone: '11999999999',
        senha: 'senha123'
    });

    let params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    let res = http.post(`${BASE_URL}/api/cliente`, payload, params);
    check(res, {
        'status is 201': (r) => r.status === 201,
        'status is 409': (r) => r.status === 409,
        'status is 400': (r) => r.status === 400,
        'status is 500': (r) => r.status === 500,
    });
    sleep(1);
}