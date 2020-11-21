
import json
import boto3

def handler(event, context):

    print(context)
    print(event)

    print('request: {}'.format(json.dumps(event)))
    return {
        'statusCode': 200,
        'headers': {
            'Content-Type': 'text/plain'
        },
        'body': 'Hello, there'
    }