const request = require('supertest');
const app = require('../server');

describe('/summary', () => {
  it('should return 200 and the expected shape', async () => {
    const response = await request(app).get('/summary');
    expect(response.status).toBe(200);
    expect(response.body).toBeDefined();
    expect(response.body.total).toBeDefined();
    expect(response.body.by_verdict).toBeDefined();
  });

  // Lab 2 Challenge: add more tests below this line
});
