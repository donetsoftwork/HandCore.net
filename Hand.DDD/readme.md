# DDD领域驱动

## 一、概念
### 1. 领域概念
#### 1.1 公域
>* 通用域

#### 1.2 私域
>* 核心域
>* 支撑域

### 2. 对象概念
|概念|简称|定义|描述|
|:--|:--|:--|:--|
|实体|Entity|Entity|具有唯一标识的对象|
|聚合|Aggregate|Aggregate|聚合|
|聚合根|Aggregate Root|Aggregate Root|聚合根|
|值对象|VO|Value Object|实体不变的复杂属性|
|DP|DP|Domain Primitive|特定领域里，拥有精准定义的VO|
|数据对象|DO|Data Object|数据类|
|持久化对象|PO|Persistent Object|数据持久化对象|
|传输对象|DTO|Data Transfer Object|数据传输对象|
|服务|Service|Service|无状态的业务逻辑封装|
|领域服务|Domain Service|Domain Service|不属于单个实体,可以跨界操作|
|规约|Specification|Specification|隐式业务规则提炼为显示概念|


## 模型
### 1. 契约
>* xxx
>* xxxEvent

#### 1.1 公共DP(Domain Primitive)
>* PhoneNumber
>* Address

### 2. DTO
>* xxxQuery
>* xxxCmd
>* xxxDto

### 3. 领域
>* xxxValue
>* xxxEntity
>* xxxRoot

#### 3.1 领域DP(Domain Primitive)
>* xxxXxx
>* xxxId
>* 让隐性的概念显性化(Make Implicit Concepts Explicit)
>* 让隐性的上下文显性化(Make Implicit Context Explicit)
>* 封装多对象行为(Encapsulate Multi-Object Behavior)

### 4. 存储
>xxxDo

## 分层
### 1. 协议层
>* 存数据类
>* 事件类

### 2. 表现层

### 3. 应用层

### 4. 领域层
#### 4.1 防腐层

### 5. 存储层

### 6. 服务层

通过IOC给领域模型注入服务
~~~java
public static Account valueOf(long phoneNo, Money amount) {
    Account account = DomainFactory.get(Account.class);
    account.setPhoneNo(phoneNo);
    account.setRemaining(amount);
    account.chargePlanList.add(new BasicChargePlan());
    return account;
}
~~~