import { IDENTITY_ENTITY_ACTION_CONTRIBUTORS } from '@abp/ng.identity';
import { IdentityUserDto } from '@abp/ng.identity/proxy';
import { EntityAction, EntityActionContributorCallback } from '@abp/ng.components/extensible';

const removePermissionAction: EntityActionContributorCallback<IdentityUserDto> = (actionList) => {
    console.log('Action list:', actionList.toArray().map(a => a.text));
    actionList.dropByValue('AbpIdentity::Permissions', (action, text) => {
        console.log(`Checking action: ${action.text} against ${text}`);
        return action.text === text || action.text?.includes('Permission');
    });
};

export const IDENTITY_CONFIG_PROVIDERS = [
    {
        provide: IDENTITY_ENTITY_ACTION_CONTRIBUTORS,
        useValue: {
            'Identity.UsersComponent': [removePermissionAction],
        },
        multi: true,
    },
];
