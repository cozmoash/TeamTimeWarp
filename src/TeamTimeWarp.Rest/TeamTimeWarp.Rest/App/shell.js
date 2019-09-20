define(['durandal/system', 'logger'],
    function(system, logger) {
        var shell = {
            activate: activate
        };
        return shell;

        function activate() {
            logger.log('started', null, system.getModuleId(shell), true);
        }
    }); 