using Lamar;
using MediatR;
using MediatR.Pipeline;

namespace Estim8.Messaging.Registry
{
    public class MessagingRegistry : ServiceRegistry
    {
        public MessagingRegistry()
        {
            //Adopted from: https://github.com/jbogard/MediatR/blob/master/samples/MediatR.Examples.Lamar
            
            //Pipeline
            //For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPreProcessorBehavior<,>));
            //For(typeof(IPipelineBehavior<,>)).Add(typeof(RequestPostProcessorBehavior<,>));
            //For(typeof(IPipelineBehavior<,>)).Add(typeof(GenericPipelineBehavior<,>));
            //For(typeof(IRequestPreProcessor<>)).Add(typeof(GenericRequestPreProcessor<>));
            //For(typeof(IRequestPostProcessor<,>)).Add(typeof(GenericRequestPostProcessor<,>));
            //For(typeof(IRequestPostProcessor<,>)).Add(typeof(ConstrainedRequestPostProcessor<,>));
            
            
            // This is the default but let's be explicit. At most we should be container scoped.
            For<IMediator>().Use<Mediator>().Transient();

            For<ServiceFactory>().Use(ctx => ctx.GetInstance);
        }

    }
}