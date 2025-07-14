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
    let res = http.get(`${BASE_URL}/api/cliente`);
    check(res, {
        'status is 200': (r) => r.status === 200,
        'status is 404': (r) => r.status === 404,
        'status is 500': (r) => r.status === 500,
    });
    sleep(1);
}