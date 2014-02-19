sqlmetal /server:192.168.0.193 /database:UMS /user:sa /password:ztf96000  /dbml:UMS.dbml /namespace:Model /views /functions /sprocs /serialization:Unidirectional  /context:_UMSDB

sqlmetal /namespace:Model /views /functions /sprocs /serialization:Unidirectional /context:_UMSDB /code:UMS.designer.cs /language:csharp UMS.dbml