import pytest
from app import app


@pytest.fixture
def client():
    app.config['TESTING'] = True
    with app.test_client() as client:
        yield client


def test_summary_returns_200_and_expected_shape(client):
    response = client.get('/summary')
    assert response.status_code == 200
    data = response.get_json()
    assert 'total' in data
    assert 'by_verdict' in data


# Lab 2 Challenge: add more tests below this line
