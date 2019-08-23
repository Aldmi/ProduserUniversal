using System;
using System.Collections.Generic;

namespace Shared
{
    /// <summary>
    /// Ответ от устройства на отправку порции данных.
    /// Содержит общую инфу и коллекцию ответов на каждый запрос ResponseDataItem
    /// </summary>
    public class ResponsePieceOfDataWrapper<TIn>
    {
        public string DeviceName { get; set; } //Название ус-ва
        public string KeyExchange { get; set; } //Ключ обмена
        public DataAction DataAction { get; set; } //Действие

        public long TimeAction { get; set; } //Время выполнения обмена (на порцию данных)
        public bool IsValidAll { get; set; } //Флаг валидности всех ответов

        public Exception ExceptionExchangePipline { get; set; } //Критическая Ошибка обработки данных в конвеере.
        public Dictionary<string, string> MessageDict { get; set; } //Доп. информация

        public List<ResponseDataItem<TIn>> ResponsesItems { get; set; } = new List<ResponseDataItem<TIn>>();
    }


    /// <summary>
    /// Единица ответа от устройства на единицу запроса
    /// </summary>
    public class ResponseDataItem<TIn>
    {
        public string RequestId { get; set; }
        public Dictionary<string, string> MessageDict { get; set; } //Доп. информация
        public StatusDataExchange Status { get; set; }
        public string StatusStr => Status.ToString();

        public InDataWrapper<TIn> RequestData { get; set; } //Данные запроса (в сыром виде)
        public Exception TransportException { get; set; } //Ошибка передачи данных

        public ResponseInfo ResponseInfo { get; set; } //Ответ
    }


    /// <summary>
    /// Контейнер-оболочка над входными данными для обменов
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    public class InDataWrapper<TIn>
    {
        public string DirectHandlerName { get; set; }       //Непосредственное имя обработчика (если == null, то логика сама выбирает обработчик )
        public List<TIn> Datas { get; set; }                //Данные
        public Command4Device Command { get; set; }         //Команды
    }


    /// <summary>
    /// Вся информация про ответ
    /// </summary>
    public class ResponseInfo
    {
        public string ResponseData { get; set; } //Ответ от устройства

        public StronglyTypedRespBase StronglyTypedResponse { get; set; } //Типизированный Ответ от устройства (Преобразованный ResponseData)

        public string Encoding { get; set; } //Кодировка ответа    
        public bool IsOutDataValid { get; set; } //Флаг валидности ответа
    }


    /// <summary>
    /// Базовый тип для создания строго типизированных ответов из строкового представления ответа.
    /// </summary>
    public class StronglyTypedRespBase
    {
        public bool IsValid { get; set; }
        public string Status { get; set; }


        public override string ToString()
        {
            return $"IsValid = {IsValid}  Status= {Status}";
        }
    }



    public enum DataAction
    {
        OneTimeAction,
        CycleAction,
        CommandAction
    }

    public enum Command4Device
    {
        None,
        On,
        Off,
        Restart,
        Clear,
        InfoEmergency
    }


    /// <summary>
    /// статус ПОСЛЕДНЕГО обмена данными
    /// </summary>
    public enum StatusDataExchange
    {
        None,
        NotOpenTransport,
        Start,
        Process,
        End,
        EndWithTimeout,
        EndWithCanceled,
        EndWithErrorCannotSendData,
        EndWithErrorWrongAnswers,
        EndWithTimeoutCritical,
        EndWithErrorCritical
    }


}